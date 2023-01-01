using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElectricityPrice : BaseEntity
    {
        public DateTime HourDK { get; set; }
        public DateTime DateDK { get; set; }
        public int HourDKNo { get; set; }
        public DateTime HourUTC { get; set; }
        public int HourUTCNo { get; set; }
        public decimal Dk1 { get; set; }
        public decimal Dk2 { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}
