using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class ElTavle : BaseEntity
    {
        public int KundeID { get; set; }
        public int? BrugerDeviceID { get; set; }
        public int TavlefabrikatID { get; set; }
        public string Adresse { get; set; }
        public string Rekvisition { get; set; }
        public int OprettetAfBrugerID { get; set; }
        public DateTime BeregnetDato { get; set; }
        public DateTime? BestiltDato { get; set; }
        public int KomponenterPris { get; set; }
        public int KomponenterInstallatoer { get; set; }
        public int TimeAntal { get; set; }
        public int TimePris { get; set; }
        public decimal DBFaktor { get; set; }
        public int Fragt { get; set; }
        public int PrisInclTimerOgDB { get; set; }
        public string Kommentar { get; set; }
        public bool Slettet { get; set; }
        public Guid ObjektGuid { get; set; }
        public int? OptjentBonus { get; set; }
        public int? UdbetaltBonus { get; set; }
        public DateTime SidstRettet { get; set; }
        public int Moduler { get; set; }
        public int KabinetModuler { get; set; }
        public int Antal { get; set; }
        public int NettoPris { get; set; }
        public bool InitialRabat { get; set; }
        public bool BonusGivende { get; set; }
        public int TypeID { get; set; }
        public int? Aar { get; set; }
        public int? LoebeNr { get; set; }
        public int? EconomicOrderNumber { get; set; }
        public int? TavleNr { get; set; }
        public int? EconomicDraftInvoiceNumber { get; set; }
        public int? EconomicBookedInvoiceNumber { get; set; }
        public DateTime? EconomicSidstRettet { get; set; }
        public string MaerkeStroem { get; set; }
        public string KapslingsKlasse { get; set; }
        public string DriftsSpaending { get; set; }
        public string MaxKortslutningsStroem { get; set; }

        public decimal InstalleretEffekt { get; set; }
        public decimal MaxEffekt { get; set; }
        public decimal Samtidighed { get; set; }

        public string CalculateTavleNr()
        {
            if (LoebeNr != null && Aar != null)
                return $"{TypeID}{Aar.Value}{LoebeNr:000}";
            else return "";
        }
    }
}
