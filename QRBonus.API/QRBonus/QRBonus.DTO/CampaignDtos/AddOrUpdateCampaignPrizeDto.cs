using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.CampaignDtos
{
    public class AddOrUpdateCampaignPrizeDto
    {
        public long[] PrizesAsc {  get; set; } = new long[0];
        public long CampaignId {  get; set; }
    }
}
