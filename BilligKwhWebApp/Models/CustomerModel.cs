using System;

namespace BilligKwhWebApp.Models
{
    public class CustomerModel : BaseModel
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

    }
}