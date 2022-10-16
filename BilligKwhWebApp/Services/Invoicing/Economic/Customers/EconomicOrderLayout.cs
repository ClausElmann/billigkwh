using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicOrderLayout
    {
        public EconomicOrderLayout() { }
        public EconomicOrderLayout(int layoutNumber)
        {
            LayoutNumber = layoutNumber;
            Self = new Uri($"https://restapi.e-conomic.com/layouts/{layoutNumber}");
        }

        public int LayoutNumber { get; set; }
        public Uri Self { get; set; }

        public readonly static EconomicOrderLayout DefaultOrderLayout = new EconomicOrderLayout(21);
    }
}