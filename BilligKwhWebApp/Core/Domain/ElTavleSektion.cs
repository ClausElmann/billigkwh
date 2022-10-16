using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElTavleSektion : BaseEntity
    {
        public int KundeID { get; set; }
        public int TavleID { get; set; }
        public int Placering { get; set; }
        public Guid ObjektGuid { get; set; }
        public int TypeID { get; set; }
        public int? HPFIKomponentID { get; set; }
    }
}
