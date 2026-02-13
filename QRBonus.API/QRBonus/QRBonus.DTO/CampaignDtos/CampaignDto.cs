using QRBonus.DTO.ProductDtos;
using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.CampaignDtos
{
    public class CampaignDto : BaseWithRegionDto
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CampaignStatus Status { get; set; }

        public List<ProductPointDto>? ProductPoints { get; set; }
    }
}