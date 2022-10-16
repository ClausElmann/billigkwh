namespace BilligKwhWebApp.Core.Domain
{
    public class LocaleStringResource : BaseEntity
    {
        public int LanguageId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }

        // Ctors
        public LocaleStringResource()
        {

        }
        public LocaleStringResource(int languageId, string resourceName, string resourceValue)
        {
            LanguageId = languageId;
            ResourceName = resourceName;
            ResourceValue = resourceValue;
        }
    }
}
