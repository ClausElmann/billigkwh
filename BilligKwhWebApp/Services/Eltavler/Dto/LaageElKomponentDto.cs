namespace BilligKwhWebApp.Core.Dto
{
    public class LaageElKomponentDto
    {
        public int Id { get; set; }
        public int? ElTavleLaageID { get; set; }
        public int TavleSektionNr { get; set; }
        public int ElTavleID { get; set; }
        public int Line { get; set; }
        public int Modul { get; set; }
        public string KomponentNavn { get; set; }
        public int KomponentID { get; set; }
        public int KomponentKategoriId { get; set; }
        public string Navn { get; set; }
        public int Placering { get; set; }
        public bool ErExtraDisp { get; set; }
        public string ServerNavn { get; set; }
        public bool AngivetNavn { get; set; }
        public int BruttoPris { get; set; }
        public int? Row { get; set; }
    }
}
