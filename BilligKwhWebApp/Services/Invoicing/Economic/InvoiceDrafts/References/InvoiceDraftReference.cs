namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.References
{
    public class InvoiceDraftReference
    {
        // Child Objects
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public InvoiceDraftReferenceCustomerContact CustomerContact { get; set; }

        // Excluded
        #region Excluded Properties
        //[JsonProperty("other")]
        //public string Other { get; set; }

        //[JsonProperty("salesPerson")]
        //public InvoiceDraftReferenceSalesPerson SalesPerson { get; set; }

        //[JsonProperty("vendorReference")]
        //public InvoiceDraftReferenceVendorReference VendorReference { get; set; }
        #endregion

        public InvoiceDraftReference()
        {
            CustomerContact = new InvoiceDraftReferenceCustomerContact();
        }
    }
}
