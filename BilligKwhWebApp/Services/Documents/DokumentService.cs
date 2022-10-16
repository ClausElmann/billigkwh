using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Dokuments.Repository;
using BilligKwhWebApp.Services.Interfaces;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services
{
    public class DokumentService : IDokumentService
    {
        // Props
        private readonly ISystemLogger _logger;
        private readonly IBaseRepository _baseRepository;
        private readonly IDokumentsRepository _documentsRepository;

        // Ctor
        public DokumentService(
            ISystemLogger logger,
            IBaseRepository baseRepository, IDokumentsRepository documentsRepository)
        {
            _logger = logger;
            _baseRepository = baseRepository;
            _documentsRepository = documentsRepository;
        }
        public IReadOnlyCollection<DokumentDto> GetAllElTavleDokumenter(int custormerId, int refTypeId, string refGuid)
        {
            return _documentsRepository.GetAllElTavleDokumenter(custormerId, refTypeId, refGuid);
        }
    }
}
