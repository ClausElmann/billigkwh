namespace BilligKwhWebApp.Services.Invoicing.Dto
{
    public class InvoiceEntryDTO
    {
        public int? EconomicId { get; set; }
        public int? CountryId { get; set; }
        public string CustomerName { get; set; }
        public int SmsCount { get; set; }
    }
}
