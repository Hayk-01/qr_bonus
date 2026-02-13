using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class Campaign : BaseWithRegionEntity
    {
        public Campaign()
        {
            ProductCampaigns = new HashSet<ProductCampaign>();
            Translations = new HashSet<CampaignTranslation>();
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CampaignStatus Status { get; set; }

        public ICollection<ProductCampaign> ProductCampaigns { get; set; }
        public ICollection<CampaignTranslation> Translations { get; set; }
    }
}
