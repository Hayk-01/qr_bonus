using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.PrizeDtos
{
    public class AddOrUpdatePrizeDto : BaseWithRegionDto
    { 
        public bool IsLeaderboard { get; set; }
        public FileDto? AddPhoto { get; set; }
        public string? Link {  get; set; }

        public List<TranslationDto> Translations { get; set; }

    }
}
