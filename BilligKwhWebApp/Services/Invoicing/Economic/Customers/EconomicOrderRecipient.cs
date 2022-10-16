namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicOrderRecipient
    {
        public EconomicOrderRecipient() { }

        public EconomicOrderRecipient(EconomicCustomer customer)
        {
            Name = customer.Name;
            Address = customer.Address;
            Zip = customer.Zip;
            City = customer.City;
            Country = customer.Country;
            Cvr = customer.CorporateIdentificationNumber;
            Attention = customer.Attention;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public EconomicCustomerContactReference Attention { get; set; }
        public EconomicVatZone VatZone { get; set; } = EconomicVatZone.DomesticVatZone;
        public string Cvr { get; set; }
    }
}