using BilligKwhWebApp.Core.Domain;
using System;

namespace BilligKwhWebApp.Services.Invoicing.Models
{
    public class InvoiceEconomicTransferResult : BaseEntity
    {
        public int EconomicId { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime AccrualStart { get; set; }
        public DateTime AccrualEnd { get; set; }
        public DateTime CreatedDateUTC { get; set; }
    }
}
