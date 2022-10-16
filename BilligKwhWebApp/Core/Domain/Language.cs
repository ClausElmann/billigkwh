namespace BilligKwhWebApp.Core.Domain
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }

        public string LanguageCulture { get; set; }

        public string UniqueSeoCode { get; set; }

        public int DefaultCurrencyId { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }
    }
}
