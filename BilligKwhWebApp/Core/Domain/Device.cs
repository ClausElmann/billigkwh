using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Print : BaseEntity
    {
        public string PrintId { get; set; }
        public int? KundeId { get; set; }
        public DateTime OprettetDatoUtc { get; set; }
        public DateTime SidsteKontaktDatoUtc { get; set; }
    }
}