using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;
using Dapper;
using System;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Caching;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Core.Toolbox;

namespace BilligKwhWebApp.Services
{
    public class PermissionService : IPermissionService
    {
        // Props
        private readonly IStaticCacheManager _cacheManager;
        private readonly IBaseRepository _baseRepository;

        // Ctor
        public PermissionService(
            IStaticCacheManager cacheManager,
            IBaseRepository baseRepository)
        {
            _cacheManager = cacheManager;
            _baseRepository = baseRepository;
        }


        #region User

        public IEnumerable<UserRole> GetAllUserRoles()
        {
            return _cacheManager.Get(CacheKeys.UserRolesAll, CacheTimeout.VeryLong, () =>
            {
                return _baseRepository.Query<UserRole>(
                    @"SELECT Id,
                        [Name],
                        IsSuperAdmin,
                        UserRoleCategoryId 
                      FROM dbo.UserRoles
                      WHERE [Name] != 'SuperAdmin'");
            });
        }
        public IEnumerable<UserRole> GetUserRolesByUser(int userId, bool isSuperAdmin)
        {
            string superAdminClause = isSuperAdmin ? " OR ur.IsSuperAdmin = 1" : "";

            return _cacheManager
                    .Get(string.Format(CacheKeys.UserRolesByUser, userId, isSuperAdmin),
                        CacheTimeout.VeryLong, () =>
                        {
                            using (var connection = ConnectionFactory.GetOpenConnection())
                            {
                                return connection.Query<UserRole>(
                                    @"SELECT ur.Id, ur.Name, ur.IsSuperAdmin, ur.UserRoleCategoryId 
                                      FROM dbo.UserRoles ur
                                      WHERE 
                                               ur.Id IN 
                                              (
                                                SELECT curm.UserRoleId 
                                                FROM dbo.CustomerUserRoleMappings curm 
                                                    INNER JOIN dbo.CustomerUserMappings cum ON cum.UserId = @UserId  AND curm.CustomerId = cum.CustomerId
                                              )
                                              
                                           
                                     " + superAdminClause, new { UserId = userId }).DistinctBy(role => role.Id).ToList();
                            }
                        });
        }
        public IEnumerable<UserRoleMapping> GetUserRoleMappingsbyUser(int userId)
        {
            return GetUserRoleMappingsByUserCached(userId);
        }

        public IEnumerable<UserRole> GetUserRoles(int userId)
        {
            var roleMappings = GetUserRoleMappingsByUserCached(userId);
            if (roleMappings != null && roleMappings.Any())
            {
                return roleMappings.Select(x => x.UserRole).ToList();
            }
            return null;
        }

        /// <summary>
        /// Returns user role mappings with the associated UserRole on each. The customerId is only used for caching purposes as we do not
        /// support having different different user role on different customers (for same user). At least not programmatically.
        /// Therefore, only mappings distinct by user role are returned.
        /// </summary>
        private IEnumerable<UserRoleMapping> GetUserRoleMappingsByUserCached(int userId)
        {
            return _cacheManager
                .Get(string.Format(CacheKeys.UserRolesByCustomerAndUser, "", userId),
                    CacheTimeout.VeryLong, () =>
                    {
                        using (var connection = ConnectionFactory.GetOpenConnection())
                        {
                            return connection.Query<UserRoleMapping, UserRole, UserRoleMapping>(
                                @"SELECT 
	                            urm.*, 
	                            0 as URMSplitter, 
	                            ur.* 
                                FROM dbo.UserRoleMappings urm 
                                    INNER JOIN dbo.UserRoles ur ON ur.Id = urm.UserRoleId
								WHERE urm.UserId = @UserId ",
                                (rolemapping, userrole) =>
                                {
                                    rolemapping.UserRole = userrole;
                                    return rolemapping;
                                },
                                new { UserId = userId },
                                splitOn: "URMSplitter")
                            .DistinctBy(m => m.UserRoleId) // As we do not support different roles on different customers (for same user), we only care about the distinct roles
                            .ToList();
                        }
                    });
        }

        public bool DoesUserHaveRole(int userId, UserRolesEnum role)
        {
            var roles = GetUserRoleMappingsByUserCached(userId);
            if (roles == null || !roles.Any())
            {
                return false;
            }

            //Enable this if we want to override the roles in terms of SuperAdmins
            //if (roles.Any(x => x.UserRole.Name.ToLowerInvariant() == UserRolePermissionProvider.SuperAdmin))
            //{
            //	return true;
            //}

            return roles.Any(x => x.UserRole.Name.ToLowerInvariant() == role.ToString().ToLowerInvariant().Trim());
        }


        public void UpdateUserRoleMapping(int customerId, int userId, int[] roleIds)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                var sql = @"    DELETE FROM dbo.UserRoleMappings 
                                WHERE NOT UserRoleId IN @roleIds 
                                AND CustomerId = @customerId 
                                AND UserId = @userId 


                                INSERT INTO dbo.UserRoleMappings(UserId,CustomerId,UserRoleId)
                                SELECT @userId, @customerId, Id
                                FROM dbo.UserRoles
                                WHERE Id IN @roleIds
                                AND NOT Id IN (SELECT UserRoleId FROM dbo.UserRoleMappings WHERE UserId = @userId AND CustomerId = @customerId ) ";

                connection.Execute(sql, new { roleIds, userId, customerId });
            }

            //Clear cache
            _cacheManager.Remove(string.Format(CacheKeys.UserRolesByCustomerAndUser, "", userId));
        }

        public void SetUserRoleAccess(int executingUserId, int customerId, int userId, List<short> roleIds, bool haveAccess)
        {
            // all roles the customer has access to
            var allowedRoleIds = GetCustomerUserRoleMappings(customerId).Select(a => (short)a.UserRoleId);

            // only if you are SuperAdmin you have to assign other roles
            if (!DoesUserHaveRole(executingUserId, UserRolesEnum.SuperAdmin))
            {
                roleIds = roleIds.Intersect(allowedRoleIds).ToList();
            }

            var existingMappings = GetUserRoleMappingsbyUser(userId);
            var mappings2DeleteOrAdd = new List<UserRoleMapping>();

            // one must never give the role SuperUser (1) from code
            roleIds.Where(a => a != 1).ForEach(roleId =>
            {
                var existingMapping = existingMappings.FirstOrDefault(m => m.UserRoleId == roleId);
                if (existingMapping != null && !haveAccess)
                {
                    mappings2DeleteOrAdd.Add(existingMapping);
                }
                else if (haveAccess && existingMapping == null)
                {
                    mappings2DeleteOrAdd.Add(new UserRoleMapping
                    {
                        CustomerId = customerId,
                        UserId = userId,
                        UserRoleId = roleId
                    });
                }
            });
            if (haveAccess)
            {
                _baseRepository.BulkInsert(mappings2DeleteOrAdd);
            }
            else
            {
                var sql = @"
                        DELETE FROM dbo.UserRoleMappings
                        WHERE Id in @Mappings";
                _baseRepository.Execute(sql, new { Mappings = mappings2DeleteOrAdd.Select(x => x.Id).ToArray() });
            }

            //Clear cache
            _cacheManager.RemoveByPattern(string.Format(CacheKeys.UserRolesByUser, userId, true));
            _cacheManager.RemoveByPattern(string.Format(CacheKeys.UserRolesByUser, userId, false));
            _cacheManager.Remove(string.Format(CacheKeys.UserRolesByCustomerAndUser, "", userId));
        }

        #endregion

        #region UserRoleGroups

        public void AddRoleGroupToUser(int groupId, int userId, int customerId)
        {
            //SQL inserts role-mappings from a specific role-group into the mapping table. It also detect existing mapping to avoid duplicates
            var sql = @"INSERT INTO UserRoleMappings(UserRoleId,UserId,CustomerId)
						SELECT RoleId As UserRoleId, @userId As UserId, @customerId As CustomerId FROM dbo.UserRolesInGroups WHERE (GroupId = @groupId) 
						AND NOT RoleId IN (SELECT existingRoles.UserRoleId FROM dbo.UserRoleMappings existingRoles WHERE (existingRoles.UserId = @userId)) ";

            _baseRepository.Execute(sql, new { groupId, userId, customerId });
        }

        public void RemoveRoleGroupOnUser(int groupId, int userId, int customerId)
        {
            //SQL deletes all role-mappings in a specific role-group from the mapping table
            var sql = @"DELETE FROM UserRoleMappings 
						WHERE UserId = @userId 
						AND CustomerId = @customerId 
						AND UserRoleId IN (SELECT RoleId FROM dbo.UserRolesInGroups WHERE GroupId = @groupId )  ";

            _baseRepository.Execute(sql, new { groupId, userId, customerId });
        }

        public void ChangeRoleGroupOnUser(int oldGroupId, int newGroupId, int userId, int customerId)
        {
            RemoveRoleGroupOnUser(oldGroupId, userId, customerId);
            AddRoleGroupToUser(newGroupId, userId, customerId);
        }

        #endregion

 
        #region Customer

        public IEnumerable<CustomerUserRoleMapping> GetCustomerUserRoleMappings(int customerId)
        {
            return _baseRepository.Query<CustomerUserRoleMapping>(
                @"SELECT 
                    Id, 
                    CustomerId, 
                    UserRoleId, 
                    DateCreatedUtc,
                    DefaultSelected
                FROM dbo.CustomerUserRoleMappings 
                WHERE CustomerId = @customerId ", new { customerId });
        }

        public void UpdateCustomerUserRoleMapping(int customerId, IEnumerable<CustomerUserRoleMapping> customerRoles)
        {
            // Map UserRoleIds 
            var roleIds = customerRoles.Select(customerRoles => customerRoles.UserRoleId);

            //Find existing CustomerRoleMappings on customer.
            var sql = @"SELECT * 
                        FROM dbo.CustomerUserRoleMappings curm
                        WHERE curm.CustomerId = @customerId ";

            // Existing and Removed mapping Ids
            var exsistingRoleMappings = _baseRepository.Query<CustomerUserRoleMapping>(sql, new { customerId = customerId });
            var newRolesMappings = customerRoles.Where(s => !exsistingRoleMappings.Select(exm => exm.UserRoleId).Contains(s.UserRoleId)).ToList();
            var updatedRoleMappings = exsistingRoleMappings.Where(s => customerRoles.Any(ex => ex.UserRoleId == s.UserRoleId && ex.DefaultSelected != s.DefaultSelected)).Select(map => map.ToggleDefaultSelected()).ToList();

            // Removed CustomerUserRole-Mapping UserId's 
            var newDefaultSelectedUserRoleId = updatedRoleMappings.Where(role => role.DefaultSelected == true).Select(role => role.UserRoleId).ToList();
            var removedRoleMappingsId = exsistingRoleMappings.Where(s => !roleIds.Contains(s.UserRoleId)).Select(map => map.UserRoleId).ToList(); // Remove UserRole on Access Revoking only
            // Remove UserRole on both Access or DefaultAccess revoking.
            //var removeDefaultSelectedUserRoleId = exsistingRoleMappings.Where(s => customerRoles.Any(ex => ex.DefaultSelected == false)).Select(map => map.UserRoleId).Concat(removedRoleMappingsId);

            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                /*
                 * CustomerUserRoleMapping
                */
                // Delete From CustomerUserRoles
                sql = @"DELETE FROM dbo.CustomerUserRoleMappings 
                        WHERE NOT UserRoleId IN @roleIds 
                            AND CustomerId = @customerId";

                // Insert Into CustomerUserRoles
                connection.Execute(sql, new { roleIds, customerId });
                connection.BulkInsert(newRolesMappings);
                connection.BulkUpdate(updatedRoleMappings);

                /*
                 * UserRole Mapping
                */
                // Remove user mappings/Roles (removed from the Customer) 
                sql = @"DELETE FROM dbo.UserRoleMappings 
                        WHERE UserRoleId IN @removedRoleMappingsId  
                        AND CustomerId = @customerId ";
                connection.Execute(sql, new { removedRoleMappingsId, customerId });

                // Add new roles to users, if they don't already exists
                sql = @"INSERT INTO UserRoleMappings(UserId, UserRoleId,CustomerId) 
                        SELECT distinct u.Id, ur.Id, u.CurrentCustomerId
                        FROM dbo.Brugere u
                        CROSS JOIN dbo.UserRoles ur
                        WHERE u.CurrentCustomerId = @customerId AND u.Deleted = 0 
                        AND ur.Id IN @newDefaultSelectedUserRoleId 
                        AND NOT ur.Id IN (SELECT UserRoleId FROM dbo.UserRoleMappings WHERE CustomerId = u.CurrentCustomerId AND UserId = u.Id) ";
                connection.Execute(sql, new { newDefaultSelectedUserRoleId, customerId });
            }

            _cacheManager.RemoveByPattern(CacheKeys.UserRolesAll);
            _cacheManager.RemoveByPattern(CacheKeys.UserRolesByUser);
            _cacheManager.RemoveByPattern(CacheKeys.UserRolesByCustomerAndUser);
        }

        public void DeleteCustomerUserRoleMapping(int customerId, int[] roleIds)
        {
            if (roleIds.Any())
            {
                //We do not clean up existing UserRoleMappings.
                var sql = @"DELETE FROM dbo.CustomerUserRoleMappings 
                            WHERE UserRoleId IN @roleIds 
                            AND CustomerId = @customerId ";
                _baseRepository.Execute(sql, new { roleIds, customerId });
            }
            //Clear cache
            _cacheManager.RemoveByPattern(CacheKeys.UserRolesByCustomerAndUser);
        }

        #endregion
    }
}
