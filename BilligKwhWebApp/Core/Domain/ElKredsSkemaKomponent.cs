using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElKredsKomponent : BaseEntity
    {
        public string VareNummer { get; set; }
        public string TypeNummer { get; set; }
        public string Beskrivelse { get; set; }
        public string KomponentText { get; set; }
        public int? Moduler { get; set; }
        public string Kategori { get; set; }
        public string Medtag { get; set; }
    }
}
