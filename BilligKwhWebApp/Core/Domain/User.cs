using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Adgangskode { get; set; }
        public int CustomerId { get; set; }
        public bool Administrator { get; set; }
        public bool SystemAdministrator { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool NoLogin { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int FailedLoginCount { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? DateLastFailedLoginUtc { get; set; }
        public DateTime? DateLastLoginUtc { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? DatePasswordResetTokenExpiresUtc { get; set; }
        public int? ImpersonatingUserId { get; set; }
        public string TimezoneId { get; set; }
        public long? ResetPhone { get; set; }
        public bool Deleted { get; set; }
        public DateTime LastEditedUtc { get; set; }

        public User() { }
         
        public User SetTidzoneId(int landId)
        {
            if (landId == CountryConstants.DanishCountryId) TimezoneId = "Romance Standard Time";
            else if (landId == CountryConstants.SwedishCountryId) TimezoneId = "Romance Standard Time";
            else if (landId == CountryConstants.FinnishCountryId) TimezoneId = "FLE Standard Time";
            else if (landId == CountryConstants.NorwegianCountryId) TimezoneId = "Romance Standard Time";
            else TimezoneId = "Romance Standard Time";

            return this;
        }
    }
}