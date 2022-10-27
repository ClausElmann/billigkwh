using System;

namespace BilligKwhWebApp.Core.Dto
{
    public class PrintDto
    {
        public int Id { get; set; }
        public string PrintId { get; set; }
        public int? KundeId { get; set; }
        public DateTime OprettetDatoUtc { get; set; }
        public DateTime SidsteKontaktDatoUtc { get; set; }
        public string Lokation { get; set; }
        public DateTime? Slettet { get; set; }
        public string Kommentar { get; set; }
    }
}
