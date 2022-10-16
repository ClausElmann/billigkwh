using System;

namespace BilligKwhWebApp.Core.Dto
{
    public class DokumentDto
    {
        public DateTime Oprettet { get; set; }
        public string Brugernavn { get; set; }
        public string FuldtNavn { get; set; }
        public string Base64Data { get; set; }
        public byte[] FilData { get; set; }
        public int? nullableInt { get; set; }
    }
}
