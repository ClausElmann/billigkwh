using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Caching;
using BilligKwhWebApp.Core.Interfaces;

namespace BilligKwhWebApp.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        // Props
        private const string _LOCALSTRINGRESOURCES_BY_LANGUAGE_KEY = "sms.lsr.language-{0}";
        private const string _LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "sms.lsr.{0}-{1}";
        private const string _LOCALSTRINGRESOURCES_PATTERN_KEY = "sms.lsr.";

        private readonly IBaseRepository _baseRepository;
        private readonly ISystemLogger _logger;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ILanguageService _languageService;

        // Ctor
        public LocalizationService(IStaticCacheManager cacheManager,
            ISystemLogger logger,
            ILanguageService languageService,
            IBaseRepository baseRepository)
        {
            _cacheManager = cacheManager;
            _logger = logger;
            _baseRepository = baseRepository;
            _languageService = languageService;
        }


        // Public Api        
        public virtual void DeleteLocaleStringResource(LocaleStringResource localeStringResource)
        {
            _baseRepository.Delete(localeStringResource);

            _cacheManager.RemoveByPattern(_LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public virtual LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId)
        {
            return _baseRepository.QueryFirstOrDefault<LocaleStringResource>(@"
						SELECT TOP (1) * FROM dbo.LocaleStringResources WHERE Id = @Id",
                        new { Id = localeStringResourceId });
        }

        public virtual LocaleStringResource GetLocaleStringResourceByName(string localeStringResourceName, int languageId)
        {
            return _baseRepository.QueryFirstOrDefault<LocaleStringResource>(@"
						SELECT TOP (1) * FROM dbo.LocaleStringResources 
                        WHERE ResourceName = @ResourceKey AND LanguageId = @LanguageId",
                                 new { ResourceKey = localeStringResourceName, LanguageId = languageId });
        }


        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            var languages = _languageService.GetAllLanguages();

            languages.ForEach(language =>
            {
                // Handle StringResource Argument
                if (localeStringResource.LanguageId == language.Id)
                {
                    _baseRepository.Insert(new LocaleStringResource
                    {
                        LanguageId = localeStringResource.LanguageId,
                        ResourceName = localeStringResource.ResourceName,
                        ResourceValue = localeStringResource.ResourceValue
                    });
                }
                else
                {
                    // Handle All other languages
                    _baseRepository.Insert(new LocaleStringResource
                    {
                        LanguageId = language.Id,
                        ResourceName = localeStringResource.ResourceName,
                        ResourceValue = "Missing Translation"
                    });
                }
            });

            _cacheManager.RemoveByPattern(_LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public virtual void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
        {
            _baseRepository.Update(localeStringResource);

            _cacheManager.RemoveByPattern(_LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public virtual IList<LocaleStringResource> GetAll(int? languageId)
        {
            var key = string.Format(CultureInfo.InvariantCulture,
                _LOCALSTRINGRESOURCES_BY_LANGUAGE_KEY,
                (languageId.HasValue && languageId.Value != 0) ? languageId.Value.ToString(CultureInfo.InvariantCulture) : "ALL");

            return _cacheManager.Get(key, () =>
            {
                return _baseRepository.Query<LocaleStringResource>(@"
					SELECT * FROM dbo.LocaleStringResources 
					WHERE  ISNULL(@LanguageId, 0) = 0 OR LanguageId = @LanguageId
					ORDER BY ResourceName",
                    new { LanguageId = languageId }).ToList();
            });
        }

        public IEnumerable<LocaleStringResource> GetAll()
        {
            var sql = @"SELECT * FROM dbo.LocaleStringResources";

            return _baseRepository.Query<LocaleStringResource>(sql);
        }

        public virtual string GetLocalizedResource(string resourceKey, int languageId)
        {
            var result = string.Empty;
            if (resourceKey == null)
            {
                resourceKey = string.Empty;
            }

            resourceKey = resourceKey.Trim().ToLowerInvariant();

            var cachekey = string.Format(_LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKey);
            var lsr = _cacheManager.Get(cachekey, () =>
            {
                var lookupResult = GetLanguageLookup(languageId)[resourceKey];

                if (lookupResult != null && lookupResult.Any())
                {
                    return lookupResult.First()?.ResourceValue;
                }
                else
                {
                    // return the resourceKey instead of empty string
                    return resourceKey;
                }
            });

            if (lsr != null)
            {
                result = lsr;
            }

            return result;
        }

        // Internals
        protected virtual void InsertLocaleStringResources(IList<LocaleStringResource> resources)
        {
            _baseRepository.Insert(resources);

            _cacheManager.RemoveByPattern(_LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        protected virtual void UpdateLocaleStringResources(IList<LocaleStringResource> resources)
        {
            _baseRepository.Update(resources);

            _cacheManager.RemoveByPattern(_LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public virtual ILookup<string, LocaleStringResource> GetLanguageLookup(int languageId)
        {
            return GetAll(languageId).ToLookup(lsr => lsr.ResourceName.ToLowerInvariant(), lsr => lsr);
        }

    }
}
