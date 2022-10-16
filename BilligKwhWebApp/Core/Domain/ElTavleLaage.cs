using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElTavleLaage : BaseEntity
    {
        public int KundeID { get; set; }
        public int TavleID { get; set; }
        public string Navn { get; set; }
        public int FabrikatId { get; set; }
        public int Bredde { get; set; }
        public int DinSkinner { get; set; }
    }
}
