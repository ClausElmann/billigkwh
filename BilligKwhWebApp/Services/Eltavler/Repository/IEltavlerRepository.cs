using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Eltavler.Repository
{
    public interface IEltavlerRepository
    {
        #region testet og i brug
        IReadOnlyCollection<SektionElKomponentDto> GetAllSektionElKomponents(int tavleId, bool onlyPlacering = false);
        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllElTavleSektionElKomponent(int tavleId);


        IReadOnlyCollection<LaageElKomponentDto> GetAllLaageElKomponents(int tavleid, int laageId, bool onlyPlacering = false);
        IReadOnlyCollection<ElTavleLaageElKomponent> GetAllElTavleLaageElKomponenter(int tavleid);

        IReadOnlyCollection<ElKredsKomponent> GetAllElKredsKomponenter();

        #endregion


        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllForPlacering(int tavleId);


        IReadOnlyCollection<ElTavleSektionElKomponent> GetAllByTavle(int tavleId);

        IReadOnlyCollection<KabinetKomponentDto> GetAllKabinetter(int tavleId);


        IReadOnlyCollection<SektionKomponentAntalDto> GetAllSektionElKomponentsAntalDto(int tavleId);
        IReadOnlyCollection<KomponentAntalEffektDto> GetAllElKomponentsAntalEffektsDto(int tavleId);

        IReadOnlyCollection<KomponentAntalEffektDto> GetAllKabinetsAntalEffektsDto(int tavleId);
     


        IReadOnlyCollection<ElTavleDto> GetAllElTavleDto(string filter, string varetype);

        IReadOnlyCollection<ElKomponentItemDto> AlleKomponenter();
        IReadOnlyCollection<ElKomponent> AlleElKomponenter();
        IReadOnlyCollection<ElTavleSektionDto> AlleSektioner(int tavleId);
        void SletExtraDispPlads(int tavleId);
        ElTavleDto GetDtoById(int tavleId);
        void Update(ElTavle eltavle);
        void Insert(ElTavle eltavle);
        ElTavle GetById(int tavleId);

        ElTavleLaage GetElTavleLaageById(int laageId);

        ElTavleSektionElKomponent GetElTavleSektionKomponentById(int id);
        void UpdateElTavleSektionKomponent(ElTavleSektionElKomponent entity);
        void InsertElTavleSektionKomponent(ElTavleSektionElKomponent entity);

        ElTavleSektionElKomponent GetElTavleSektionElKomponentById(int id);
        void UpdateElTavleSektionElKomponent(ElTavleSektionElKomponent entity);

        void UpdateModuler(int eltavleId);
        void DeleteElTavleSektionKomponent(int id);
        int SektionKomponentMaxPlacering(int? tavleSektionId, int tavleId);
        void FlytKunde(int eltavleId, int kundeId, int brugerId);
        int NextLoebeNr(int aar, int typeId);
    }
}
