using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElTavleLaageElKomponent : BaseEntity
    {
        public int KundeID { get; set; }
        public int ElTavleID { get; set; }
        public int? ElTavleLaageId { get; set; }
        public int TavleSektionNr { get; set; }
        public int Placering { get; set; }
        public int KomponentID { get; set; }
        public string Navn { get; set; }
        public int Line { get; set; }
        public bool ErExtraDisp { get; set; }
        public bool AngivetNavn { get; set; }
        public int? Row { get; set; }
    }
}
