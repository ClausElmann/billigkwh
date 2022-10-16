using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ApplicationSetting : BaseEntity
    {
        public int ApplicationSettingTypeId { get; set; }

        public string ApplicationSettingName { get; set; }

        public string Setting { get; set; }

        public DateTime DateLastUpdatedUtc { get; set; }
    }
}
