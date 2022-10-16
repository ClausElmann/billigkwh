namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public partial class BookInvoice
    {
        public DraftInvoice DraftInvoice { get; set; }
    }

    public partial class DraftInvoice
    {
        public int DraftInvoiceNumber { get; set; }
    }

    public class EconomicOrder
    {
        public int? OrderNumber { get; set; }
        public int? DraftInvoiceNumber { get; set; }
        public int? BookedInvoiceNumber { get; set; }
        public EconomicCustomerReference Customer { get; set; }
        public string Currency { get; set; }
        public string Date { get; set; }
        public EconomicOrderLayout Layout { get; set; } = EconomicOrderLayout.DefaultOrderLayout;
        public EconomicPaymentTerms PaymentTerms { get; set; }
        public EconomicOrderRecipient Recipient { get; set; }
        public EconomicOrderNotes Notes { get; set; }
        public EconomicOrderLine[] Lines { get; set; }
        public EconomicOrderReferences References { get; set; }
    }
}
