using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Enums;

namespace BilligKwhWebApp.Services
{
    public interface IApplicationSettingService
    {
        ApplicationSetting Get(AppSettingEnum settingType, string defaultSetting = null);
        ApplicationSetting GetUncached(AppSettingEnum settingType, string defaultSetting = null);
        void Save(ApplicationSetting setting);
    }
}
