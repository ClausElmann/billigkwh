using System;

namespace BilligKwhWebApp.Core.Domain
{
    public enum InstillingEnum : int
    {
        //EltavleDB = 15,
        //EltavleTimePris = 16,
    }

    public class Indstilling : BaseEntity
    {
        public int KundeID { get; set; }
        public int? BrugerID { get; set; }
        public int InstillingEnumID { get; set; }
        public int? _Int { get; set; }
        public DateTime? _DateTime { get; set; }
        public string _String { get; set; }
        public decimal? _Double { get; set; }
        public bool? _Bool { get; set; }
        public bool Slettet { get; set; }
        public DateTime SidstRettet { get; set; }
    }
}
