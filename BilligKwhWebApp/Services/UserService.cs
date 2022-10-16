using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Domain.ValueObjects;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Localization;
using BilligKwhWebApp.Services.QueryWrappers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BilligKwhWebApp.Services
{
    public class UserService : IUserService
    {
        // Props 
        private readonly IBaseRepository _baseRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ISystemLogger _logger;

        // Ctor
        public UserService(
            ICustomerService customerService,
            ILocalizationService localizationService,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            IPermissionService permissionService,
            IBaseRepository baseRepository,
            ISystemLogger logger,
            IStaticCacheManager cacheManager)
        {
            _localizationService = localizationService;
            _customerService = customerService;
            _permissionService = permissionService;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _baseRepository = baseRepository;
            _logger = logger;
            _cacheManager = cacheManager;
        }

        public Bruger Get(int userId, bool inclDeleted = false)
        {
            return _baseRepository.QueryFirstOrDefault<Bruger>(
                @"SELECT * 
                    FROM dbo.Bruger 
                    WHERE Id = @Id 
                    AND (@InclDeleted = 1 OR Slettet <> 1)",
                new { Id = userId, InclDeleted = inclDeleted });
        }
        public IEnumerable<Bruger> GetUsers()
        {
            return _baseRepository.Query<Bruger>(
                @"SELECT * 
                    FROM dbo.Bruger users");
        }
        public IEnumerable<Bruger> GetUsers(int customerId)
        {
            return _baseRepository.Query<Bruger>(
                @"SELECT * 
	                    FROM dbo.Bruger users
	                    WHERE users.CurrentCustomerId = @CustomerId",
            new { CustomerId = customerId });
        }
        public IEnumerable<Bruger> GetUsers(IEnumerable<int> userIds)
        {
            var param = new { UserIds = userIds };
            var sql = @"SELECT *
                        FROM dbo.Bruger
                        WHERE Id IN @UserIds";

            return _baseRepository.Query<Bruger>(sql, param);
        }

        public IList<Bruger> GetUsersByCustomer(int customerId, bool onlyDeleted = false, int? userId = null)
        {
            var query = _baseRepository.Query<Bruger>(@"		
					SELECT * FROM dbo.Bruger  
					WHERE (VærtKundeID = @CustomerId AND 
						((@OnlyDeleted = 1 AND Slettet = 1) OR (@OnlyDeleted = 0 AND Slettet <> 1))) or (@UserId IS NOT NULL AND Id = @UserId)",
                        new { CustomerId = customerId, OnlyDeleted = onlyDeleted, UserId = userId });

            return query.ToList();
        }

        //public IEnumerable<Index_User> GetUsersForSuperAdmin(int roleFilterId, bool reverseFilter = false)
        //{
        //    string sql;

        //    if (roleFilterId < 1)
        //    {
        //        sql = @"SELECT
        //                    [user].Id AS Id,
        //                 [user].[Name] AS Name,
        //                 [user].Email AS Email,
        //                 [user].CountryId AS CountryId,
        //                    [user].Slettet AS Deleted,
        //                    [user].DateLastLoginUtc AS LastLoggedIn,
        //                 customer.[Name] AS Customer,
        //                 customer.Id AS CustomerId                     
        //                FROM dbo.Bruger [user]
        //                 INNER JOIN dbo.Kunde customer ON customer.Id = [user].CurrentCustomerId
        //                ORDER BY customer.[Name] ASC";

        //        return _baseRepository.Query<Index_User>(sql);
        //    }

        //    string equalityOperator = reverseFilter ? "=0" : "<>0";

        //    var param = new { RoleFilterId = roleFilterId };

        //    sql = @$"SELECT [user].Id AS Id,
        //                   [user].[Name] AS Name,
        //                   [user].Email AS Email,
        //                   [user].CountryId AS CountryId,
        //                   [user].Slettet AS Deleted,
        //                   [user].DateLastLoginUtc AS LastLoggedIn,
        //                   customer.[Name] AS Customer,
        //                   customer.Id AS CustomerId
        //            FROM dbo.Bruger [user]
        //            INNER JOIN dbo.Kunde customer ON customer.Id = [user].CurrentCustomerId
        //            WHERE
        //                (SELECT count(*)
        //                 FROM dbo.UserRoleMappings AS urm
        //                 INNER JOIN dbo.UserRoles AS ur ON urm.UserRoleId = ur.Id
        //                 WHERE (urm.UserId = [user].Id
        //                        AND ur.id = @RoleFilterId)) {equalityOperator}
        //            ORDER BY customer.[Name] ASC";
        //    return _baseRepository.Query<Index_User>(sql, param);
        //}

        public IEnumerable<UserRoleMapping> GetUserRoles(int userId)
        {
            var param = new { UserId = userId };
            var sql = @"SELECT *
                        FROM dbo.UserRoleMappings userRoleMapping
                            INNER JOIN dbo.UserRoles userRole ON UserRole.Id = userRoleMapping.UserRoleId
                        WHERE UserId = @UserId";

            return _baseRepository.Query<UserRoleMapping, UserRole>(sql, (userRoleMapping, userRole) =>
            {
                userRoleMapping.UserRole = userRole;
                return userRoleMapping;
            }, param);
        }

        public IEnumerable<Bruger> GetUsersWithRoles(int customerId)
        {
            var param = new { CustomerId = customerId };
            var sql = @"SELECT
                            1 AS PseudoId,
	                        [user].*,
	                        [userRole].*
	                    FROM dbo.Bruger [user]
	                        INNER JOIN dbo.UserRoleMappings userRoleMapping ON userRoleMapping.UserId = [user].Id
	                        INNER JOIN dbo.UserRoles userRole ON userRoleMapping.UserRoleId = userRole.Id
                        WHERE [user].CurrentCustomerId = @CustomerId AND [user].Id = 67";

            var userWrapperLookup = new Dictionary<int, UserWrapper>();
            var userLookUp = new Dictionary<int, Bruger>();

            var result = _baseRepository.Query<UserWrapper, Bruger, UserRole>(sql, (userWrapper, user, userRole) =>
            {
                // Lookup Wrapper
                if (!userWrapperLookup.TryGetValue(userWrapper.PseudoId, out var currentWrapper))
                {
                    userWrapperLookup.Add(userWrapper.PseudoId, currentWrapper = userWrapper);
                }
                // Lookup Users
                if (!userLookUp.TryGetValue(user.Id, out var currentUser))
                {
                    userLookUp.Add(user.Id, currentUser = user);
                    currentWrapper.Users.Add(currentUser);
                }

                return userWrapper;
            }, "PseudoId, Id, Id, Id", param).FirstOrDefault().Users;

            return result;
        }

        public Bruger GetUserByEmail(string brugernavn)
        {

           // _logger.Fatal($"Failed to send E-mail", null);

            if (string.IsNullOrWhiteSpace(brugernavn))
            {
                return null;
            }
            var user = _baseRepository.QueryFirstOrDefault<Bruger>(
                @"SELECT * 
                  FROM dbo.Bruger 
                  WHERE Brugernavn = @Brugernavn AND Slettet <> 1 and NoLogin <> 1",
                new { brugernavn = brugernavn.Trim() });

            return user;
        }

        public bool UserExists(string email)
        {
            return GetUserByEmail(email) != null;
        }

        public void Create(Bruger user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            user.SidstRettet = DateTime.UtcNow;
            _baseRepository.Insert(user);
        }

        public void Update(Bruger user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (user.Id == 0)
                throw new ArgumentNullException(nameof(user));

            user.SidstRettet = DateTime.UtcNow;
            _baseRepository.Update(user);
        }

        public bool CanUserAccessUser(int userId, int idOfUserToAccess)
        {
            return _baseRepository.Query<int?>(@"
                select null 
                    from CustomerUserMappings c1 
                    inner join CustomerUserMappings c2 on c1.CustomerId = c2.CustomerId 
                    where c1.UserId = @userId and c2.UserId = @idOfUserToAccess",
                    new { userId, idOfUserToAccess }).Any();
        }

        public void CountFailedLoginTry(Bruger user)
        {
            user.DateLastFailedLoginUtc = DateTime.UtcNow;
            user.FailedLoginCount = (short)Math.Min(user.FailedLoginCount + 1, short.MaxValue);

            Update(user);
        }
        public bool IsUserLocked(Bruger user)
        {
            // 5 Tries in 5min intervals
            if (user.IsLockedOut || (user.DateLastFailedLoginUtc.HasValue && user.FailedLoginCount > 5) &&
               (DateTime.UtcNow - user.DateLastFailedLoginUtc).Value.Minutes < (user.FailedLoginCount - 5))
            {
                if (!user.IsLockedOut)
                {
                    user.IsLockedOut = true;
                    Update(user);
                }
                return true;
            }
            return false;
        }
        public void UnlockLogIn(Bruger user)
        {
            user.FailedLoginCount = 0;
            user.DateLastFailedLoginUtc = null;
            user.IsLockedOut = false;
            user.SidstRettet = DateTime.UtcNow;
            _baseRepository.Update(user);
        }

        public void SendTwoFactorPinCodeByEmail(Bruger user, int pinCode)
        {
            // Get Template and Que Email Message
            var mailTemplate = _emailTemplateService.GetMasterTemplate(user.SprogID);
            var subjectText = _localizationService.GetLocalizedResource("login.TwofactorEmailSubject", user.SprogID);
            var bodyText = _localizationService.GetLocalizedResource("login.TwofactorEmailText", user.SprogID)
                            .Replace("[code]", pinCode.ToString(CultureInfo.InvariantCulture).AddSpacesBetweenCharacters(), StringComparison.OrdinalIgnoreCase)
                            .Replace("[name]", user.FuldtNavn, StringComparison.OrdinalIgnoreCase);

            var emailMessage = new EmailMessage
            {
                CustomerId = user.AktivKundeID,
                ToEmail = user.Brugernavn,
                ToName = user.FuldtNavn,
                FromEmail = "e-mail@noreply.nu",
                FromName = "BilligKwh",
                ReplyTo = "e-mail@noreply.nu",
                Subject = subjectText,
                Body = mailTemplate.MergeMasterMainContent(bodyText).Html,
                UseBcc = false,
                HasAttachments = false,
            };

            // Save and Send Email
            _emailService.Save(emailMessage);
        }


        public Result<Bruger> Excist(EmailAddress emailAddress)
        {
            var user = _baseRepository.QueryFirstOrDefault<Bruger>(
                        @"SELECT * 
                          FROM dbo.Bruger 
                          WHERE Email = @Email",
                        new { emailAddress.Email });

            if (user != null)
            {
                return Result.Ok(user);
            }
            return Result.Fail<Bruger>("No User Excist");
        }
    }
}
