using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Invoicing.Dto
{
    public class UpdateInvoiceDraftDTO
    {
        public IEnumerable<EconomicReportDTO> EconomicReports { get; set; }
        public EconomicReportDTO CustomerEconomicReport { get; set; }
    }
}
