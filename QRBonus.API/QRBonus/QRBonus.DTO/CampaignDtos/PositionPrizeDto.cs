using QRBonus.DTO.PrizeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.CampaignDtos
{
    public class PositionPrizeDto
    {
        public PrizeDto Prize { get; set; }
        public int Position { get; set; }

    }
}
