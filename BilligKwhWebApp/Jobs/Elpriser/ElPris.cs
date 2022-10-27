using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Jobs.ElectricityPrices
{
    public class Record
    {
        public DateTime HourUTC { get; set; }
        public DateTime HourDK { get; set; }
        public string PriceArea { get; set; }
        public double SpotPriceDKK { get; set; }
        public double SpotPriceEUR { get; set; }
    }

    public class Root
    {
        public int total { get; set; }
        public string sort { get; set; }
        public string dataset { get; set; }
        public List<Record> records { get; set; }
    }
}


