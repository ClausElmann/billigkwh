using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Dokuments.Repository
{
    public interface IDokumentsRepository
    {
        IReadOnlyCollection<DokumentDto> GetAllElTavleDokumenter(int custormerId, int refTypeID, string refGuid);
    }
}
