using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElKomponent : BaseEntity
    {
        public string Navn { get; set; }
        public int KategoriID { get; set; }
        public int Placering { get; set; }
        public int Modul { get; set; }
        public int DinSkinner { get; set; }
        public DateTime SidstRettet { get; set; }
        public bool Slettet { get; set; }
        public int KostKomponent { get; set; }
        public int KostHjaelpeMat { get; set; }
        public int KostLoen { get; set; }
        public int BruttoPris { get; set; }
        public int DaekningsBidrag { get; set; }
        public bool HPFI { get; set; }
        public bool KombiRelae { get; set; }
        public bool UdenBeskyttelse { get; set; }
        public int MontageMinutter { get; set; }
        public decimal Effekt { get; set; }
    }

}
