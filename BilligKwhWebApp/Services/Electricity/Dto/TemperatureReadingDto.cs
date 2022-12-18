using System;

namespace BilligKwhWebApp.Services.Electricity.Dto
{
    public class TemperatureReadingDto
    {
        public int Id { get; set; }
        public DateTime DatetimeUtc { get; set; }
        public int DeviceId { get; set; }
        public decimal Temperature { get; set; }
        public bool IsRunning { get; set; }
    }
}
