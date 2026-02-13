using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.PrizeDtos
{
    public class PrizeDto : BaseWithRegionDto
    {
        public string Name { get; set; }
        public bool IsLeaderboard { get; set; }
        public string? Link { get; set; }
    }  
}