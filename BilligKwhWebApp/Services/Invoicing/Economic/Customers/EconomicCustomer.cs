using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicCustomer
    {
        // Required fields for create
        public string Name { get; set; }
        public string Currency { get; set; }
        public EconomicCustomerGroup CustomerGroup { get; set; }
        public EconomicPaymentTerms PaymentTerms { get; set; }
        public EconomicVatZone VatZone { get; set; } = EconomicVatZone.DomesticVatZone;
        
        // Optional fields for create
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Address { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string City { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Zip { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Website { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CorporateIdentificationNumber { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TelephoneAndFaxNumber { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Country { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Ean { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MobilePhone { get; set; }

        public bool EInvoicingDisabledByDefault { get; set; }

        // Required fields for update
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CustomerNumber { get; set; }

        // Optional fields for update
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EconomicCustomerContactReference Attention { get; set; }
    }
}
