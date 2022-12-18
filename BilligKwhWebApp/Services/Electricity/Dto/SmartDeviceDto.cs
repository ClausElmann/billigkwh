using System;

namespace BilligKwhWebApp.Services.Electricity.Dto
{
    public class SmartDeviceDto
    {
        public int Id { get; set; }
        public string Uniqueidentifier { get; set; }
        public int? CustomerId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime LatestContactUtc { get; set; }
        public string Location { get; set; }
        public int ZoneId { get; set; }
        public decimal MaxRate { get; set; }
        public DateTime? Deleted { get; set; }
        public string Comment { get; set; }
        public bool DisableWeekends { get; set; }
        public int StatusId { get; set; }
        public int? MinTemp { get; set; }
        public decimal? MaxRateAtMinTemp { get; set; }
        public string ErrorMail { get; set; }
    }
}
