using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class CampaignPrize : BaseEntity
    {
        public long CampaignId { get; set; }
        public long PrizeId { get; set; }
        public int Position { get; set; }

        public Campaign? Campaign { get; set; }
        public Prize? Prize { get; set; }
    }
}
