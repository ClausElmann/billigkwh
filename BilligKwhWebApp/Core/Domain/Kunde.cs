using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Kunde : BaseEntity
    {
        public string Kundenavn { get; set; }
        public string Adresse { get; set; }
        public string Kontakt { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public short PostNr { get; set; }
        public string By { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public short KundeTypeID { get; set; }
        public bool Skjult { get; set; }
        public bool Slettet { get; set; }
        public DateTime SidstRettet { get; set; }
        public Guid KundeGuid { get; set; }
        public short BrancheTypeID { get; set; }
        public int SprogID { get; set; }
        public string Kontaktperson { get; set; }
        public string KundeOverskrift { get; set; }
        public int LandID { get; set; }
        public int? Cvr { get; set; }
        public string TidzoneId { get; set; }
        public int? EconomicId { get; set; }
        public string FakturaMail { get; set; }
        public string FakturaKontaktPerson { get; set; }
        public string FakturaTelefonFax { get; set; }
        public string FakturaMobil { get; set; }

        public Kunde SetTidzoneId(int countryId)
        {
            if (countryId == CountryConstants.DanishCountryId) TidzoneId = "Romance Standard Time";
            else if (countryId == CountryConstants.SwedishCountryId) TidzoneId = "Romance Standard Time";
            else if (countryId == CountryConstants.FinnishCountryId) TidzoneId = "FLE Standard Time";
            else if (countryId == CountryConstants.NorwegianCountryId) TidzoneId = "Romance Standard Time";
            else TidzoneId = "Romance Standard Time";

            return this;
        }
    }
}
