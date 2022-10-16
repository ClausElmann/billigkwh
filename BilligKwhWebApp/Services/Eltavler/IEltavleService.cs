using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Interfaces
{
    public partial interface IEltavleService
    {
        #region testet og i brug
        IReadOnlyCollection<SektionElKomponentDto> GetAllSektionElKomponents(int tavleId, bool onlyPlacering = false);
        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionElKomponent(int tavleId);
        void GemKomponentPlaceringer(int tavleId, IEnumerable<SektionElKomponentDto> komponentPlaceringer, bool fromFrontEnd = true);

        #region laage
        IReadOnlyCollection<LaageElKomponentDto> GetAllLaageElKomponents(int tavleid, int laageId, bool onlyPlacering = false);
        IReadOnlyCollection<ElTavleLaageElKomponent> GetAllElTavleLaageElKomponent(int laageId);
        void GemLaageKomponentPlaceringer(int tavleId, IEnumerable<LaageElKomponentDto> komponentPlaceringer, bool fromFrontEnd = true);

        IReadOnlyCollection<ElKredsKomponent> GetAllElKredsKomponenter();

        #endregion


        #endregion

        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionKomponenterByTavle(int tavleId);
        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionKomponenterForPlacering(int tavleId);
        IReadOnlyCollection<KabinetKomponentDto> GetAllKabinetter(int tavleId);
        IReadOnlyCollection<SektionKomponentAntalDto> GetAllSektionElKomponentsAntalDto(int tavleId);

        IReadOnlyCollection<KomponentAntalEffektDto> GetAllElKomponentsAntalEffektsDto(int tavleId);
        IReadOnlyCollection<KomponentAntalEffektDto> GetAllKabinetsAntalEffektsDto(int tavleId);

        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllKomponentPlacering(int tavleId);
        IReadOnlyCollection<ElTavleDto> GetAllElTavleDto(string filter, string varetype);
        IReadOnlyCollection<ElKomponentItemDto> AlleKomponenter();
        IReadOnlyCollection<ElKomponent> AlleElKomponenter();

        IReadOnlyCollection<ElTavleSektionDto> AlleSektioner(int tavleId);

        void SletExtraDispPlads(int tavleId);

        ElTavleDto GetDtoById(int tavleId);
        ElTavle GetById(int tavleId);
        ElTavleLaage GetElTavleLaageById(int laageId);

        void Update(ElTavle entity);
        void Insert(ElTavle entity);


        ElTavleSektionElKomponent GetElTavleSektionElKomponentById(int id);
        void UpdateElTavleSektionElKomponent(ElTavleSektionElKomponent entity);


        ElTavleSektionElKomponent GetElTavleSektionKomponentById(int id);
        void UpdateElTavleSektionKomponent(ElTavleSektionElKomponent entity);
        void InsertElTavleSektionKomponent(ElTavleSektionElKomponent entity);
        void GenberegnKabinetter(int eltavleId, int? moduler);
        void GenberegnKomponenterPris(int eltavleId);
        void GenberegnVarmetab(int tavleId);

        void UpdateModuler(int eltavleId);

        void DeleteElTavleSektionKomponent(int id);

        int SektionKomponentMaxPlacering(int? tavleSektionId, int tavleId);

        Task<int> CreateOrUpdateOrder(ElTavle eltavle);

        Task<int> CreateOrUpdateInvoiceDraft(ElTavle eltavle);

        Task<int> BookInvoice(ElTavle eltavle);

        void FlytKunde(int eltavleId, int kundeId, int brugerId);
        int NextLoebeNr(int aar, int typeId);


    }
}
