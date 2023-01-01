using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class KwhConsumption : BaseEntity
    {
        public DateTime DateDK { get; set; }
        public int HourDK { get; set; }
        public int DeviceId { get; set; }
        public long Counter { get; set; }
        public decimal Consumption { get; set; }
    }
}
