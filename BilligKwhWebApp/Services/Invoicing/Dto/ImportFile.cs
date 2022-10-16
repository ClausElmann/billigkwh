using Microsoft.AspNetCore.Http;
using System;

namespace BilligKwhWebApp.Services.Invoicing.Dto
{
    public class ImportFileDTO
    {
        public IFormFile File { get; set; }
        public DateTime DateFromUTC { get; set; }
        public DateTime DateToUTC { get; set; }
    }
}
