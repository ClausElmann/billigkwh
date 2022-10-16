using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Komponenter.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Komponenter.Repository
{
    public interface IKomponenterRepository
    {
        IReadOnlyCollection<ElKomponentDto> GetAllKomponentDto(string filter, int komponentKategoriId);
        ElKomponentDto GetDtoById(int komponentId);
        ElKomponent GetById(int komponentId);
        void Update(ElKomponent entity);
        void Insert(ElKomponent entity);
    }
}
