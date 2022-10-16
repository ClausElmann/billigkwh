using BilligKwhWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace BilligKwhWebApp.Infrastructure.RootFolder
{
    public class RootFolderService : IRootFolderService
    {
        private readonly IWebHostEnvironment webhostEnvironment;

        public RootFolderService(IWebHostEnvironment webhostEnvironment)
        {
            this.webhostEnvironment = webhostEnvironment;
        }
        public string GetRootFolder()
        {
            return webhostEnvironment.ContentRootPath;
        }
    }
}
