using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElPris : BaseEntity
    {
        public DateTime DatoUtc { get; set; }
        public int TimeDk { get; set; }
        public decimal Dk1 { get; set; }
        public decimal Dk2 { get; set; }
        public DateTime Updated { get; set; }
    }
}
