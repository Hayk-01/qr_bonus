using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class CampaignTranslation: BaseTranslationEntity
    {
        public long CampaignId { get; set; }
        public string? Name { get; set; }

        public Campaign? Campaign { get; set; }

    }
}
