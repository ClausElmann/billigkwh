using System;

namespace BilligKwhWebApp.Models
{
    public class BilligKwhModel
    {
        public DateTime ServerTid { get; set; }
        public bool[] Today { get; set; }
        public bool[] Tomorrow { get; set; }
        public string DeviceID { get; set; }
    }
}
