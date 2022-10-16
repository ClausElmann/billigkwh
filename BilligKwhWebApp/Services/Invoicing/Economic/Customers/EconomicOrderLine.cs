namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicOrderLine
    {
        public string Description { get; set; }
        public EconomicProduct Product { get; set; }
        public EconomicUnit Unit { get; set; } = EconomicUnit.StkEconomicUnit;
        public int LineNumber { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal UnitNetPrice { get; set; }
    }
}