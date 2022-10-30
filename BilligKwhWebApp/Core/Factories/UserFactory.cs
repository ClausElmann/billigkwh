using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BilligKwhWebApp.Core.Factories
{
    public partial class UserFactory : IUserFactory
    {
        // Props
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IAuthenticationService _authenticationService;

        // Ctor
        public UserFactory(IPermissionService permissionService,
            IWorkContext workContext,
            ILocalizationService localizationService,
            IAuthenticationService authenticationService)
        {
            _permissionService = permissionService;
            _workContext = workContext;
            _localizationService = localizationService;
            _authenticationService = authenticationService;
        }


        // Public Api
        public UserEditModel PrepareUserModel(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var model = new UserEditModel
            {
                Id = user.Id,
                CustomerId = user.CustomerId,
                NewPassword = user.Adgangskode,
                CountryId = user.CountryId,
                Email = user.Email,
                Name = user.Name,
                LanguageId = user.LanguageId,
                Administrator = user.Administrator,
                Phone = user.Phone,
                Deleted = user.Deleted,
            };

            return model;
        }


        //public Bruger InitForCreate(string firstName, string lastName, Kunde customer, string brugernavn)
        //{
        //    if (customer == null) return null;
        //    var user = new Bruger
        //    {
        //        Adgangskode = "",
        //        BrugernavnUtfCode = _authenticationService.BeregnBrugernavnHashUtfCode(brugernavn),
        //        BrugernavnUnicode = _authenticationService.BeregnBrugernavnHashUnicode(brugernavn),
        //        VærtKundeID = customer.Id,
        //        AktivKundeID = customer.Id,
        //        ErAdministrator = true,
        //        SystemAdministrator = false,
        //        KundeAdministrator = false,
        //        Fornavn = firstName,
        //        Efternavn = lastName,
        //        Telefon = "",
        //        Mobil = "",
        //        NoLogin = false,
        //        SprogID = customer.LanguageId != 0 ? customer.LanguageId : CountryConstants.DanishLanguageId,
        //        SidstRettet = DateTime.UtcNow,
        //        SidstRettetAfBrugerID = -1,
        //        Slettet = false,
        //        StandardBedriftID = 0,
        //        SecurityStamp = "",
        //        PasswordHash = "",
        //        PortalAdministrator = false,
        //        LandID = customer.LandID != 0 ? customer.LandID : CountryConstants.DanishCountryId,
        //        Password = "",
        //        PasswordSalt = "",
        //        FailedLoginCount = 0,
        //        IsLockedOut = false,
        //        TidzoneId = customer.TidzoneId ?? "Romance Standard Time"
        //    };

        //    return user;
        //}


        public User CreateUserEntity(UserEditModel model, Customer customer)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            if (customer == null) return null;

            var user = new User
            {
                Id = model.Id,
                Email = model.Email,
                Adgangskode = model.NewPassword,
                CustomerId = customer.Id,
                Administrator = model.Administrator,
                SystemAdministrator = false,
                Name = model.Name,
                Phone = model.Phone,
                NoLogin = false,
                LanguageId = model.LanguageId,
                CountryId = model.CountryId,
                Password = "",
                PasswordSalt = "",
                FailedLoginCount = 0,
                IsLockedOut = false,
                TimezoneId = "Romance Standard Time",
            };

            return user.SetTidzoneId(model.LanguageId);
        }

        public UserRoleModel PrepareUserRoleModel(UserRole role, int languageId)
        {
            return new UserRoleModel
            {
                Id = role.Id,
                Name = role.Name,
                NameLocalized = _localizationService.GetLocalizedResource($"Roles.User.Name.{role.Name}", languageId),
                Description = _localizationService.GetLocalizedResource($"Roles.User.Description.{role.Name}", languageId),
                Category = _localizationService.GetLocalizedResource($"administration.Users.RoleCategory.Enum.Name.{role.UserRoleCategoryId}", languageId)
            };
        }

        public IList<UserRoleAccessModel> PrepareUserRoleAccessModels(int userId, IList<UserRole> roles, IList<UserRoleMapping> mappings, int languageId)
        {
            var userRoleModels = from r in roles
                                 select new UserRoleAccessModel
                                 {
                                     HasAccess = mappings.FirstOrDefault(m => m.UserId == userId && m.UserRoleId == r.Id) != null,
                                     UserRole = new UserRoleModel
                                     {
                                         Id = r.Id,
                                         Description = _localizationService.GetLocalizedResource($"Roles.User.Description.{r.Name}", languageId),
                                         Name = r.Name,
                                         NameLocalized = _localizationService.GetLocalizedResource($"Roles.User.Name.{r.Name}", languageId),
                                         Category = _localizationService.GetLocalizedResource($"administration.Users.RoleCategory.Enum.Name.{r.UserRoleCategoryId}", languageId)
                                     }
                                 };

            return userRoleModels.OrderBy(m => m.UserRole.NameLocalized).ToList();
        }
    }
}
