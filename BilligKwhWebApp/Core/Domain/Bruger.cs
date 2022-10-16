using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Bruger : BaseEntity
    {
        public string Brugernavn { get; set; }
        public string Adgangskode { get; set; }
        public Guid? BrugernavnUtfCode { get; set; }
        public Guid? BrugernavnUnicode { get; set; }
        public int VærtKundeID { get; set; }
        public int AktivKundeID { get; set; }
        public bool ErAdministrator { get; set; }
        public bool SystemAdministrator { get; set; }
        public bool KundeAdministrator { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string FuldtNavn { get; set; }
        public string Telefon { get; set; }
        public string Mobil { get; set; }
        public bool NoLogin { get; set; }
        public int SprogID { get; set; }
        public DateTime SidstRettet { get; set; }
        public int SidstRettetAfBrugerID { get; set; }
        public bool Slettet { get; set; }
        public int StandardBedriftID { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public bool PortalAdministrator { get; set; }
        public DateTime? PersonDataAcceptDato { get; set; }
        public int LandID { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int FailedLoginCount { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? DateLastFailedLoginUtc { get; set; }
        public DateTime? DateLastLoginUtc { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? DatePasswordResetTokenExpiresUtc { get; set; }
        public int? ImpersonatingUserId { get; set; }
        public string TidzoneId { get; set; }
        public long? ResetPhone { get; set; }

        public Bruger() { }

        //public Bruger InitForCreate(string firstName, string lastName, Kunde customer, string brugernavn)
        //{
        //    if (customer == null) return null;

        //    Adgangskode = "";
        //    BrugernavnUtfCode =  Guid.NewGuid();
        //    BrugernavnUnicode = Guid.NewGuid(); 
        //    VærtKundeID = customer.Id;
        //    AktivKundeID = customer.Id;
        //    ErAdministrator = true;
        //    SystemAdministrator = false;
        //    KundeAdministrator = false;
        //    Fornavn = firstName;
        //    Efternavn = lastName;
        //    Telefon = "";
        //    Mobil = "";
        //    NoLogin = false;
        //    SprogID = customer.SprogID != 0 ? customer.SprogID : CountryConstants.DanishLanguageId;
        //    SidstRettet = DateTime.UtcNow;
        //    SidstRettetAfBrugerID = -1;
        //    Slettet = false;
        //    StandardBedriftID = 0;
        //    SecurityStamp = "";
        //    PasswordHash = "";
        //    PortalAdministrator = false;
        //    LandID = customer.LandID != 0 ? customer.LandID : CountryConstants.DanishCountryId; ;
        //    Password = "";
        //    PasswordSalt = "";
        //    VærtKundeID = customer.Id;
        //    AktivKundeID = customer.Id;
        //    FailedLoginCount = 0;
        //    IsLockedOut = false;
        //    TidzoneId = customer.TidzoneId ?? "Romance Standard Time"; ;
        //    return this;
        //}
        //public Result<Bruger> UpdateFromForm(string firstName, string lastName, string brugernavn)
        //{
        //    Fornavn = firstName;
        //    Efternavn = lastName;
        //    Brugernavn = brugernavn;
        //    return Result.Ok(this);
        //}
        //public Result<Bruger> UpdateFromForm(string firstName, string lastName, string brugernavn, int languageId)
        //{
        //    // Hard Check on Country Id (dto-Model).
        //    if (languageId == CountryConstants.DanishLanguageId ||
        //        languageId == CountryConstants.SwedishLanguageId ||
        //        languageId == CountryConstants.EnglishLanguageId ||
        //        languageId == CountryConstants.FinnishLanguageId ||
        //        languageId == CountryConstants.NorwegianLanguageId)
        //    {
        //        Fornavn = firstName;
        //        Efternavn = lastName;
        //        Brugernavn = brugernavn;
        //        LandID = languageId;// CountryId and LanguageId are the same, don't change it.
        //        SprogID = languageId;

        //        return Result.Ok(this); ;
        //    }
        //    return Result.Fail<Bruger>("CountryId not valid");
        //}

        public Bruger SetTidzoneId(int landId)
        {
            if (landId == CountryConstants.DanishCountryId) TidzoneId = "Romance Standard Time";
            else if (landId == CountryConstants.SwedishCountryId) TidzoneId = "Romance Standard Time";
            else if (landId == CountryConstants.FinnishCountryId) TidzoneId = "FLE Standard Time";
            else if (landId == CountryConstants.NorwegianCountryId) TidzoneId = "Romance Standard Time";
            else TidzoneId = "Romance Standard Time";

            return this;
        }
    }
}