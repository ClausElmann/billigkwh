using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Recipe : BaseEntity
    {
        public int CustomerId { get; set; }
        public int DeviceId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public decimal MaxRate { get; set; }
        public int ZoneId { get; set; }
    }
}
