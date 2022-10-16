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

        public CustomerModel CreateCustomerModel(Kunde entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            return new CustomerModel
            {
                Id = entity.Id,
                Name = entity.Kundenavn,
                Address = entity.Adresse,
                DisplayAddress = entity.Adresse + ", " + entity.PostNr + " " + entity.By,
                Zipcode = entity.PostNr,
                Deleted = entity.Slettet,
                LanguageId = entity.SprogID,
                CountryId = entity.LandID,
                CompanyRegistrationId = entity.Cvr,
                TimeZoneId = entity.TidzoneId,
                InvoiceMail = entity.FakturaMail,
                InvoiceContactPerson = entity.FakturaKontaktPerson,
                InvoicePhoneFax = entity.FakturaTelefonFax,
                InvoiceMobile = entity.FakturaMobil,
                City = entity.By,
                EconomicId = entity.EconomicId,
            };
        }

        public Kunde CreateCustomerEntity(CustomerModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var kunde = new Kunde
            {
                Id = model.Id,
                Kundenavn = model.Name,
                Adresse = model.Address,
                Kontakt = "",
                Telefon = "",
                Fax = "",
                Email = "",
                PostNr = model.Zipcode,
                By = model.City,
                Lat = 0,
                Lon = 0,
                KundeTypeID = 12,
                Skjult = false,
                Slettet = model.Deleted,
                KundeGuid = new Guid(),
                BrancheTypeID = 11,
                SprogID = model.LanguageId,
                Kontaktperson = "",
                KundeOverskrift = "",
                LandID = model.CountryId,
                Cvr = model.CompanyRegistrationId,
                TidzoneId = model.TimeZoneId,
                FakturaMail = model.InvoiceMail,
                FakturaKontaktPerson = model.InvoiceContactPerson,
                FakturaTelefonFax = model.InvoicePhoneFax,
                FakturaMobil = model.InvoiceMobile,
                EconomicId = model.EconomicId
            };

            return kunde.SetTidzoneId(kunde.LandID);
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
