using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.BannerDtos
{
    public class AddOrUpdateBannerDto : BaseWithRegionDto
    {
        public DateTime? ExpirationDate { get; set; }
        public FileDto? AddPhoto { get; set; }
        public string? Link { get; set; }

        public List<BannerTranslationDto> Translations { get; set; }
    }
}
