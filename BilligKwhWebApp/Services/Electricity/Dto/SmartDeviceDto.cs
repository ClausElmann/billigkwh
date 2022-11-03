using System;

namespace BilligKwhWebApp.Services.Electricity.Dto
{
    public class SmartDeviceDto
    {
        public int Id { get; set; }
        public string SmartDeviceId { get; set; }
        public int? KundeId { get; set; }
        public DateTime OprettetDatoUtc { get; set; }
        public DateTime SidsteKontaktDatoUtc { get; set; }
        public string Lokation { get; set; }
        public int ZoneId { get; set; }
        public decimal MaxRate { get; set; }
        public DateTime? Slettet { get; set; }
        public string Kommentar { get; set; }
    }
}
