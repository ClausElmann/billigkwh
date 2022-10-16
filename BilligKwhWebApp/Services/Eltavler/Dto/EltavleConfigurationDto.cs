using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Core.Dto
{
    public class EltavleConfigurationDto
    {
        public IEnumerable<SektionElKomponentDto> Komponenter { get; set; }
        public int AntalSkinner { get; set; }
        public int ModulPrSkinne { get; set; }
        public ElTavleDto ElTavle { get; set; }
    }
}
