using System;

namespace BilligKwhWebApp.Models
{
    public class ElectricityPriceModel
    {
        public int Id { get; set; }
        public DateTime HourUTC { get; set; }
        public int HourDKNo { get; set; }
        public decimal Dk1 { get; set; }
        public decimal Dk2 { get; set; }
    }
}
