namespace BilligKwhWebApp.Core.Domain.Invoicing
{
    public class InvoiceEntry : BaseEntity
    {
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int EconomicId { get; set; }
        public int SmsCount { get; set; }
        // Foreign Keys
        public int InvoiceFileId { get; set; }

        // Ctor
        public InvoiceEntry()
        {

        }
        public InvoiceEntry(string customerName, int economicId, int smsCount, int invoiceFileId)
        {
            CustomerName = customerName;
            EconomicId = economicId;
            SmsCount = smsCount;
            InvoiceFileId = invoiceFileId;
        }
    }
}
