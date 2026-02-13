using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.BannerDtos
{
    public class BannerDto : BaseWithRegionDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
    }
}
