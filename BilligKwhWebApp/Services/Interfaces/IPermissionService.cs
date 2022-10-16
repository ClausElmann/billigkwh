using BilligKwhWebApp.Core;
using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface IPermissionService
    {
 
        #region User
        bool DoesUserHaveRole(int userId, UserRolesEnum role);
        IEnumerable<UserRole> GetUserRoles(int userId);

        IEnumerable<UserRole> GetAllUserRoles();
        IEnumerable<UserRole> GetUserRolesByUser(int userId, bool isSuperAdmin);
        IEnumerable<UserRoleMapping> GetUserRoleMappingsbyUser(int userId);
        void SetUserRoleAccess(int executingUserId, int customerId, int userId, List<short> roleIds, bool haveAccess);

        void UpdateUserRoleMapping(int customerId, int userId, int[] roleIds);

        void AddRoleGroupToUser(int groupId, int userId, int customerId);

        void ChangeRoleGroupOnUser(int oldGroupId, int newGroupId, int userId, int customerId);
        #endregion


        #region Customer
        IEnumerable<CustomerUserRoleMapping> GetCustomerUserRoleMappings(int customerId);

        void UpdateCustomerUserRoleMapping(int customerId, IEnumerable<CustomerUserRoleMapping> customerUserRoles);
        void DeleteCustomerUserRoleMapping(int customerId, int[] roleIds);
        #endregion
    }

}
