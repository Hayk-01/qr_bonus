using QRBonus.DTO.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.CampaignDtos
{
    public class LeaderboardDto 
    {
        public long Position { get; set; }
        public long Points { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
