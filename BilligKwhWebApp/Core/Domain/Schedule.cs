using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Schedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public int DeviceId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public DateTime? LastReadUtc { get; set; }
        public bool H00 { get; set; }
        public bool H01 { get; set; }
        public bool H02 { get; set; }
        public bool H03 { get; set; }
        public bool H04 { get; set; }
        public bool H05 { get; set; }
        public bool H06 { get; set; }
        public bool H07 { get; set; }
        public bool H08 { get; set; }
        public bool H09 { get; set; }
        public bool H10 { get; set; }
        public bool H11 { get; set; }
        public bool H12 { get; set; }
        public bool H13 { get; set; }
        public bool H14 { get; set; }
        public bool H15 { get; set; }
        public bool H16 { get; set; }
        public bool H17 { get; set; }
        public bool H18 { get; set; }
        public bool H19 { get; set; }
        public bool H20 { get; set; }
        public bool H21 { get; set; }
        public bool H22 { get; set; }
        public bool H23 { get; set; }
    }
}
