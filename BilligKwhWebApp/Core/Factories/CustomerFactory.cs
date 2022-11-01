using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Localization;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BilligKwhWebApp.Core.Factories
{
    public class CustomerFactory : ICustomerFactory
    {
        private readonly ILocalizationService _localizationService;

        public CustomerFactory(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public CustomerModel CreateCustomerModel(Customer entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            return new CustomerModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                Deleted = entity.Deleted,
                LanguageId = entity.LanguageId,
                CountryId = entity.CountryId,
                CompanyRegistrationId = entity.CompanyRegistrationId,
                TimeZoneId = entity.TimeZoneId,
                DateCreatedUtc = entity.DateCreatedUtc,
            };
        }

        public Customer CreateCustomerEntity(CustomerModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var kunde = new Customer
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Deleted = model.Deleted,
                LanguageId = model.LanguageId,
                CountryId = model.CountryId,
                CompanyRegistrationId = model.CompanyRegistrationId + "",
                TimeZoneId = model.TimeZoneId,
                Active = model.Active,
                DateCreatedUtc = DateTime.UtcNow,
                DateDeletedUtc = model.DateDeletedUtc,
                DateLastUpdatedUtc = DateTime.UtcNow,
                PublicId = model.PublicId,
            };

            return kunde.SetTidzoneId(kunde.CountryId);
        }

        public IList<UserRoleAccessModel> PrepareCustomerUserRoleAccessModels(int customerId, IList<UserRole> roles, IList<CustomerUserRoleMapping> mappings, int languageId)
        {
            return roles
                .Select(r => new UserRoleAccessModel
                {
                    DefaultSelected = mappings.FirstOrDefault(m => m.CustomerId == customerId && m.UserRoleId == r.Id)?.DefaultSelected ?? false,
                    HasAccess = mappings.FirstOrDefault(m => m.CustomerId == customerId && m.UserRoleId == r.Id) != null,
                    UserRole = new UserRoleModel
                    {
                        Id = r.Id,
                        Description = _localizationService.GetLocalizedResource($"Roles.User.Description.{r.Name}", languageId),
                        Name = r.Name,
                        NameLocalized = _localizationService.GetLocalizedResource($"Roles.User.Name.{r.Name}", languageId),
                        Category = _localizationService.GetLocalizedResource($"administration.Users.RoleCategory.Enum.Name.{r.UserRoleCategoryId}", languageId)
                    }
                })
                .OrderBy(m => m.UserRole.NameLocalized).ToList();
        }

        public IList<UserRoleAccessModel> PrepareCustomerUserRoleAccessModelsForSuperAdmin(int customerId, IList<UserRole> roles, IList<CustomerUserRoleMapping> mappings, int languageId)
        {
            return roles
                .Select(r => new UserRoleAccessModel
                {
                    DefaultSelected = mappings.FirstOrDefault(m => m.CustomerId == customerId && m.UserRoleId == r.Id)?.DefaultSelected ?? false,
                    HasAccess = mappings.FirstOrDefault(m => m.CustomerId == customerId && m.UserRoleId == r.Id) != null,
                    UserRole = new UserRoleModel
                    {
                        Id = r.Id,
                        Description = _localizationService.GetLocalizedResource($"Roles.User.Description.{r.Name}", languageId),
                        Name = r.Name,
                        NameLocalized = _localizationService.GetLocalizedResource($"Roles.User.Name.{r.Name}", languageId),
                        Category = _localizationService.GetLocalizedResource($"administration.Users.RoleCategory.Enum.Name.{r.UserRoleCategoryId}", languageId)
                    }
                })
                .OrderBy(m => m.UserRole.NameLocalized).ToList();
        }

        public IEnumerable<CustomerUserRoleMapping> PrepareCustomerUserRoleMappings(int customerId, IEnumerable<UserRoleAccessModel> accessModels)
        {
            return accessModels
                .Where(roleDto => roleDto.HasAccess == true)
                .Select(userRoleAccessModel => new CustomerUserRoleMapping
                {
                    DateCreatedUtc = DateTime.UtcNow,
                    DefaultSelected = userRoleAccessModel.DefaultSelected,
                    UserRoleId = userRoleAccessModel.UserRole.Id,
                    CustomerId = customerId
                });
        }
    }

}
