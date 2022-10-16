using System.Collections.Generic;
using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services.Localization
{
    public partial interface ILocalizationService
    {
        LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId);
        LocaleStringResource GetLocaleStringResourceByName(string localeStringResourceName, int languageId);

        IEnumerable<LocaleStringResource> GetAll();
        IList<LocaleStringResource> GetAll(int? languageId);

        void InsertLocaleStringResource(LocaleStringResource localeStringResource);
        void UpdateLocaleStringResource(LocaleStringResource localeStringResource);
        void DeleteLocaleStringResource(LocaleStringResource localeStringResource);



        /// <summary>
        /// Returns a specific localized resource. Note: if resource is not found, the returned value will be the value of the resourceKey.
        /// </summary>
        string GetLocalizedResource(string resourceKey, int languageId);

    }
}
