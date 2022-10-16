using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{
    public partial interface IDokumentService
    {
        IReadOnlyCollection<DokumentDto> GetAllElTavleDokumenter(int custormerId, int refTypeId, string refGuid);
    }
}
