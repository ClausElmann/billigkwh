using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Komponenter.Repository;
using BilligKwhWebApp.Services.Interfaces;
using System.Collections.Generic;
using BilligKwhWebApp.Services.Enums;
using System;
using System.Threading.Tasks;
using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
using BilligKwhWebApp.Services.Customers;
using System.Linq;
using BilligKwhWebApp.Services.Komponenter.Dto;

namespace BilligKwhWebApp.Services
{
    public class KomponentService : IKomponentService
    {
        // Props
        private readonly ISystemLogger _logger;
        private readonly IKomponenterRepository _komponenterRepository;
        private readonly IBaseRepository _baseRepository;
  
        // Ctor
        public KomponentService(
            ISystemLogger logger,
            IKomponenterRepository komponenterRepository,
            IBaseRepository baseRepository)
        {
            _logger = logger;
            _komponenterRepository = komponenterRepository;
            _baseRepository = baseRepository;
        }

        public IReadOnlyCollection<ElKomponentDto> AlleElKomponenter(string filter,int komponentKategoriId)
        {
            return _komponenterRepository.GetAllKomponentDto(filter, komponentKategoriId);
        }

        public ElKomponent GetById(int komponentId)
        {
            return _komponenterRepository.GetById(komponentId);
        }

        public ElKomponentDto GetDtoById(int komponentId)
        {
            return _komponenterRepository.GetDtoById(komponentId);
        }

        public void Insert(ElKomponent entity)
        {
            _komponenterRepository.Insert(entity);
        }

        public void Update(ElKomponent entity)
        {
            _komponenterRepository.Update(entity);
        }
    }
}
