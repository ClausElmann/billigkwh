using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class SensitivePageLoad : BaseEntity
    {
        public string IP { get; set; }
        public string PageNameId { get; set; }
        public DateTime LoadDateTimeUtc { get; set; }
    }
}
