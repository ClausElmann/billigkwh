using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class TemperatureReading : BaseEntity
    {
        public DateTime DatetimeUtc { get; set; }
        public int DeviceId { get; set; }
        public decimal Temperature { get; set; }
    }
}
