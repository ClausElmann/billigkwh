using System;

namespace BilligKwhWebApp.Models
{
    public class ElprisModel
    {
        public int Id { get; set; }
        public DateTime DatoUtc { get; set; }
        public int TimeDk { get; set; }
        public decimal Dk1 { get; set; }
        public decimal Dk2 { get; set; }
    }
}
