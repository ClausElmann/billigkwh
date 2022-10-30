using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Enums;
using BilligKwhWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BilligKwhWebApp.Core
{
    public partial class WebWorkContext : IWorkContext
    {
        // Cache
        private User _cachedUser;
        private Customer _cachedCustomer;
        private int? _cachedUserId;
        private int? _cachedCustomerId;
        private int? _cachedProfileId;
        private int? _cachedImpersonateFromUserId;

        // Props
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;


        // Ctor
        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            IPermissionService permissionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _authenticationService = authenticationService;
            _customerService = customerService;
            _permissionService = permissionService;
        }

        // Props
        public bool IsLoggedIn
        {
            get
            {
                return CurrentUser != null;
            }
        }

        public bool IsImpersonating
        {
            get
            {
                return CurrentUserId != ImpersonateFromUserId;
            }
        }

        public virtual User CurrentUser
        {
            get
            {
                if (_cachedUser != null && _cachedUser.Id == CurrentUserId)
                    return _cachedUser;

                var user = _userService.Get(CurrentUserId);

                if (user == null || user.Deleted)
                    return null;

                _cachedUser = user;

                return _cachedUser;
            }
            set
            {
                _cachedUser = value;
            }
        }

        public virtual int CurrentUserId
        {
            get
            {
                if (_cachedUserId != null)
                    return (int)_cachedUserId;

                var userId = _authenticationService.GetClaimValue(AccessTokenClaims.UserId);

                _cachedUserId = userId;

                return userId;
            }
            set
            {
                _cachedUserId = value;
            }
        }

        public virtual int CurrentProfileId
        {
            get
            {
                if (_cachedProfileId != null)
                    return (int)_cachedProfileId;

                var profileId = _authenticationService.GetClaimValue(AccessTokenClaims.ProfileId);

                _cachedProfileId = profileId;

                return profileId;
            }
            set
            {
                _cachedProfileId = value;
            }
        }

        public virtual Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null && _cachedCustomer.Id == CustomerId)
                    return _cachedCustomer;

                var customer = _customerService.Get(CustomerId);

                if (customer == null || customer.Deleted)
                    return null;

                _cachedCustomer = customer;

                return _cachedCustomer;
            }
            set
            {
                _cachedCustomer = value;
            }
        }

        public virtual int CustomerId
        {
            get
            {
                if (_cachedCustomerId != null)
                    return (int)_cachedCustomerId;

                var customerId = _authenticationService.GetClaimValue(AccessTokenClaims.CustomerId);

                _cachedCustomerId = customerId;

                return (int)_cachedCustomerId;
            }
            set
            {
                _cachedCustomerId = value;
            }
        }


        public virtual int ImpersonateFromUserId
        {
            get
            {
                if (_cachedImpersonateFromUserId != null)
                    return (int)_cachedImpersonateFromUserId;

                var impersonateFromUserId = _authenticationService.GetClaimValue(AccessTokenClaims.ImpersonateFromUserId);

                _cachedImpersonateFromUserId = impersonateFromUserId;

                return (int)_cachedImpersonateFromUserId;
            }
            set
            {
                _cachedImpersonateFromUserId = value;
            }
        }

        #region Roles
        public bool IsUserSuperAdmin()
        {
            return _permissionService.DoesUserHaveRole(CurrentUser.Id, UserRolesEnum.SuperAdmin);
        }

        public bool UserHaveRole(UserRolesEnum role)
        {
            if (CurrentUser == null)
                return false;

            return _permissionService.DoesUserHaveRole(CurrentUser.Id, role);
        }
     
        #endregion
    }
}
