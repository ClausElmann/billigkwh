using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services.Komponenter.Dto
{
    public class ElKomponentDto
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public int KategoriId { get; set; }
        public int Placering { get; set; }
        public int Modul { get; set; }
        public int DinSkinner { get; set; }
        public bool Slettet { get; set; }
        public int KostKomponent { get; set; }
        public int KostHjaelpeMat { get; set; }
        public int KostLoen { get; set; }
        public int BruttoPris { get; set; }
        public int DaekningsBidrag { get; set; }
        public bool HPFI { get; set; }
        public bool KombiRelae { get; set; }
        public bool UdenBeskyttelse { get; set; }
        public int MontageMinutter { get; set; }
        public decimal Effekt { get; set; }

        public static ElKomponentDto ConvertToDTO(ElKomponent entity)
        {
            return new ElKomponentDto
            {
                Id = entity.Id,
                Navn = entity.Navn,
                KategoriId = entity.KategoriID,
                Placering = entity.Placering,
                Modul = entity.Modul,
                DinSkinner = entity.DinSkinner,
                Slettet = entity.Slettet,
                KostKomponent = entity.KostKomponent,
                KostHjaelpeMat = entity.KostHjaelpeMat,
                KostLoen = entity.KostLoen,
                BruttoPris = entity.BruttoPris,
                DaekningsBidrag = entity.DaekningsBidrag,
                HPFI = entity.HPFI,
                KombiRelae = entity.KombiRelae,
                UdenBeskyttelse = entity.UdenBeskyttelse,
                MontageMinutter = entity.MontageMinutter,
                Effekt = entity.Effekt,
            };
        }
    }
}
