using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BilligKwhWebApp.Jobs
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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


