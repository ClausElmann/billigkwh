namespace BilligKwhWebApp.Services.Invoicing.EconomicDTO.Infrastructure
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public string FirstPage { get; set; }
        public string NextPage { get; set; }
        public string LastPage { get; set; }
        public int Results { get; set; }
    }
}
