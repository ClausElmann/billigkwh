using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElTavleSektionElKomponent : BaseEntity
    {
        public int KundeID { get; set; }
        public int ElTavleID { get; set; }
        public int ElTavleSektionID { get; set; }
        public int KomponentID { get; set; }
        public Guid? ObjektGuidApp { get; set; }
        public int Placering { get; set; }
        public DateTime SidstRettet { get; set; }
        public string Navn { get; set; }
        public int Line { get; set; }
        public bool ErExtraDisp { get; set; }
        public bool AngivetNavn { get; set; }
    }
}
