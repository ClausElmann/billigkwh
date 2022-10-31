using BilligKwhWebApp.Core.Domain;
using System;

namespace BilligKwhWebApp.Services.Settings
{
    public class ApplicationSetting : BaseEntity
    {
        public int ApplicationSettingTypeId { get; set; }

        public string ApplicationSettingName { get; set; }

        public string Setting { get; set; }

        public DateTime DateLastUpdatedUtc { get; set; }
    }
}
