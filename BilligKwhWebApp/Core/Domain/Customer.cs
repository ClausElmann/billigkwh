using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Customer : BaseEntity
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateLastUpdatedUtc { get; set; }
        public string TimeZoneId { get; set; }
        public int LanguageId { get; set; }
        public string CompanyRegistrationId { get; set; }
        public DateTime LastEditedUtc { get; set; }

        public Customer SetTidzoneId(int countryId)
        {
            if (countryId == CountryConstants.DanishCountryId) TimeZoneId = "Romance Standard Time";
            else if (countryId == CountryConstants.SwedishCountryId) TimeZoneId = "Romance Standard Time";
            else if (countryId == CountryConstants.FinnishCountryId) TimeZoneId = "FLE Standard Time";
            else if (countryId == CountryConstants.NorwegianCountryId) TimeZoneId = "Romance Standard Time";
            else TimeZoneId = "Romance Standard Time";

            return this;
        }
    }
}
