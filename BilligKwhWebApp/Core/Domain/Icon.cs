namespace BilligKwhWebApp.Core.Domain
{
    public class Icon : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconType { get; set; }
        public string Tooltip { get; set; }
        public string DefaultColor { get; set; }
        public int CustomerId { get; set; }
    }
}
