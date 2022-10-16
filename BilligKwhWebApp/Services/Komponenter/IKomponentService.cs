using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Services.Komponenter.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Interfaces
{
    public partial interface IKomponentService
    {
        IReadOnlyCollection<ElKomponentDto> AlleElKomponenter(string filter, int komponentKategoriId);
        ElKomponentDto GetDtoById(int komponentId);
        ElKomponent GetById(int komponentId);
        void Update(ElKomponent entity);
        void Insert(ElKomponent entity);
    }
}
