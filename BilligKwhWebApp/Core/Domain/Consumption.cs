using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Consumption : BaseEntity
    {
        public DateTime Date { get; set; }
        public int DeviceId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public long H00 { get; set; }
        public long H01 { get; set; }
        public long H02 { get; set; }
        public long H03 { get; set; }
        public long H04 { get; set; }
        public long H05 { get; set; }
        public long H06 { get; set; }
        public long H07 { get; set; }
        public long H08 { get; set; }
        public long H09 { get; set; }
        public long H10 { get; set; }
        public long H11 { get; set; }
        public long H12 { get; set; }
        public long H13 { get; set; }
        public long H14 { get; set; }
        public long H15 { get; set; }
        public long H16 { get; set; }
        public long H17 { get; set; }
        public long H18 { get; set; }
        public long H19 { get; set; }
        public long H20 { get; set; }
        public long H21 { get; set; }
        public long H22 { get; set; }
        public long H23 { get; set; }
    }
}
