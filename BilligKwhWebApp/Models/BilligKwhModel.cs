using System;

namespace BilligKwhWebApp.Models
{
    public class BilligKwhModel
    {
        public int Second { get; set; }
        public int Minute { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool[] Today { get; set; }
        public bool[] Tomorrow { get; set; }
        public string DeviceID { get; set; }
    }
}
