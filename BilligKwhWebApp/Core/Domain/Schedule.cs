using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Schedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public int DeviceId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public DateTime? LastReadUtc { get; set; }
        public int H00 { get; set; }
        public int H01 { get; set; }
        public int H02 { get; set; }
        public int H03 { get; set; }
        public int H04 { get; set; }
        public int H05 { get; set; }
        public int H06 { get; set; }
        public int H07 { get; set; }
        public int H08 { get; set; }
        public int H09 { get; set; }
        public int H10 { get; set; }
        public int H11 { get; set; }
        public int H12 { get; set; }
        public int H13 { get; set; }
        public int H14 { get; set; }
        public int H15 { get; set; }
        public int H16 { get; set; }
        public int H17 { get; set; }
        public int H18 { get; set; }
        public int H19 { get; set; }
        public int H20 { get; set; }
        public int H21 { get; set; }
        public int H22 { get; set; }
        public int H23 { get; set; }
    }
}
