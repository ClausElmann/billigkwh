namespace BilligKwhWebApp.Services.Invoicing.Models
{
    public class EconomicFileEntry
    {
        // Props
        public string Kunde { get; set; }
        public int? EconomicId { get; set; }
        public int SMSantal { get; set; }
        public bool IsValid { get; private set; }

        // Ctor
        public EconomicFileEntry()
        {

        }

        public EconomicFileEntry SetIsValid()
        {
            if (EconomicId.HasValue && EconomicId != 0) IsValid = true;
            else IsValid = false; 

            return this;
        }
    }
}
