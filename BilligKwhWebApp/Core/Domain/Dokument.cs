using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Dokument : BaseEntity
    {
        public int KundeID { get; set; }
        public Guid ObjektGuid { get; set; }
        public int RefTypeID { get; set; }
        public Guid RefGuid { get; set; }
        public int FiltypeID { get; set; }
        public string Filnavn { get; set; }
        public string Beskrivelse { get; set; }
        public byte[] FilData { get; set; }
        public int FilStørrelse { get; set; }
        public int? BrugerDeviceID { get; set; }
        public DateTime Oprettet { get; set; }
        public int OprettetAfBrugerID { get; set; }
        public decimal Xkoordinat { get; set; }
        public decimal Ykoordinat { get; set; }
        public bool Slettet { get; set; }
        public int? ModulDokumentID { get; set; }
        public int? HelpdeskID { get; set; }
        public int? InstillingEnumID { get; set; }
        public string Info { get; set; }
        public Guid? DokumentGuid { get; set; }
        public string RefObjektGuid { get; set; }
        public int? RefID { get; set; }
        public string DeviceID { get; set; }
    }
}