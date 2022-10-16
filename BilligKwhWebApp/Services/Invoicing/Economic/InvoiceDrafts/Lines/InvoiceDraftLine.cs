using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines
{
    public class InvoiceDraftLine
    {
        [JsonPropertyName("lineNumber")]
        public int LineId { get; set; }

        // Properties
        #region Properties
        public string Description { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal MarginInBaseCurrency { get; set; }

        public decimal MarginPercentage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? UnitCostPrice { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? UnitNetPrice { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Quantity { get; set; }

        public int SortKey { get; set; }
        #endregion

        // Child Objects
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public InvoiceDraftLineProduct Product { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public InvoiceDraftLineUnit Unit { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public InvoiceDraftLineAccrual Accrual { get; set; }


        public InvoiceDraftLine()
        {
            Unit = new InvoiceDraftLineUnit();
            Product = new InvoiceDraftLineProduct();
            Accrual = new InvoiceDraftLineAccrual();
        }
        public InvoiceDraftLine(int lineId, int sortKey, string description, decimal quantity, decimal unitNetPrice,
            InvoiceDraftLineUnit unit, InvoiceDraftLineProduct product, InvoiceDraftLineAccrual accrual)
        {
            LineId = lineId;
            SortKey = sortKey;
            Description = description;
            Quantity = quantity;
            UnitNetPrice = unitNetPrice;
            Unit = unit;
            Product = product;
            Accrual = accrual;
        }

    }
}
