using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class SmartDevice : BaseEntity
    {
        public string Uniqueidentifier { get; set; }
        public int? CustomerId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime LatestContactUtc { get; set; }
        public string Location { get; set; }
        public int ZoneId { get; set; }
        public decimal MaxRate { get; set; }
        public DateTime? Deleted { get; set; }
        public string Comment { get; set; }
        public int Delay { get; set; }
        public int DebugMinutes { get; set; }
    }
}