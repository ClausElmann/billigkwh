namespace BilligKwhWebApp.Models
{
    public partial class LanguageModel : BaseModel
    {
        public string Name { get; set; }

        public string LanguageCulture { get; set; }

        public string UniqueSeoCode { get; set; }
    }
}
