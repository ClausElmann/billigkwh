using BilligKwhWebApp.Core.Caching;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BilligKwhWebApp.Services
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        // Props
        private readonly IBaseRepository _baseRepository;
        private readonly IStaticCacheManager _cacheManager;
        private const string _APPLICATIONSETTINGS_KEY = "Sms.applicationsettings";

        // Ctor
        public ApplicationSettingService(IStaticCacheManager cacheManager,
            IBaseRepository baseRepository)
        {
            _cacheManager = cacheManager;
            _baseRepository = baseRepository;
        }

        // Public Api
        public ApplicationSetting GetUncached(AppSettingEnum settingType, string defaultSetting = null)
        {
            var setting = _baseRepository.Query<ApplicationSetting>("SELECT * FROM dbo.ApplicationSettings where ApplicationSettingTypeId = @settingType", new { settingType }).FirstOrDefault();

            if (setting == null)
            {
                setting = CreateDefault(settingType, defaultSetting);
                Save(setting);
            }

            return setting;
        }

        public ApplicationSetting Get(AppSettingEnum settingType, string defaultSetting = null)
        {
            var settings = AllSettings();

            if ((!settings.TryGetValue(settingType, out var setting) || setting == null) && defaultSetting != null)
            {
                setting = CreateDefault(settingType, defaultSetting);
                Save(setting);
            }
            return setting;
        }

        private static ApplicationSetting CreateDefault(AppSettingEnum settingType, string defaultSetting)
        {
            return new ApplicationSetting
            {
                ApplicationSettingTypeId = (int)settingType,
                DateLastUpdatedUtc = DateTime.UtcNow,
                Setting = defaultSetting,
            };
        }

        public void Save(ApplicationSetting setting)
        {
            if (setting is null)
                throw new ArgumentNullException(nameof(setting));

            var dbSetting = Get((AppSettingEnum)setting.ApplicationSettingTypeId);

            setting.DateLastUpdatedUtc = DateTime.UtcNow;
            setting.ApplicationSettingName = ((AppSettingEnum)setting.ApplicationSettingTypeId).ToString();

            if (dbSetting == null)
            {
                _baseRepository.Insert(setting);
            }
            else
            {
                _baseRepository.Update(setting);
            }

            _cacheManager.RemoveByPattern(_APPLICATIONSETTINGS_KEY);
        }

        // Internals
        private IDictionary<AppSettingEnum, ApplicationSetting> AllSettings()
        {
            return _cacheManager.Get(_APPLICATIONSETTINGS_KEY, CacheTimeout.FifteenMinutes, () =>
            {
                return _baseRepository
                    .Query<ApplicationSetting>("SELECT * FROM dbo.ApplicationSettings")
                    .ToDictionary(f => (AppSettingEnum)f.ApplicationSettingTypeId, v => v);
            });
        }
    }

}
