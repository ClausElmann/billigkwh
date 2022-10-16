namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines
{
    public class InvoiceDraftLineProduct
    {
        public string ProductNumber { get; set; }

        public InvoiceDraftLineProduct()
        {

        }
        public InvoiceDraftLineProduct(string productNumber)
        {
            ProductNumber = productNumber;
        }
    }
}
