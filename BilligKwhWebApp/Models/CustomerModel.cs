namespace BilligKwhWebApp.Models
{
    public class CustomerModel : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public short Zipcode { get; set; }
        public string City { get; set; }
        public string DisplayAddress { get; set; }
        public bool Deleted { get; set; }
        //public string PublicId { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public int? CompanyRegistrationId { get; set; }
        public string TimeZoneId { get; set; }
        public int HourWage { get; set; }
        public double CoveragePercentage { get; set; }
        public int? EconomicId { get; set; }
        public string InvoiceMail { get; set; }
        public string InvoiceContactPerson { get; set; }
        public string InvoicePhoneFax { get; set; }
        public string InvoiceMobile { get; set; }
    }
}