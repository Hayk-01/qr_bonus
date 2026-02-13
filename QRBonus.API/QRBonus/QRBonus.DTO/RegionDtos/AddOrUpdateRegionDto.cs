using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.RegionDtos
{
    public class AddOrUpdateRegionDto : BaseDto
    {
        public string AreaCode { get; set; }

        public List<TranslationDto> Translations { get; set; }

    }
}
