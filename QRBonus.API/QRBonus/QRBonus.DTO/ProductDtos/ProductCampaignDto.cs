using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.ProductDtos
{
    public class ProductCampaignDto
    {
        public long ProductId { get; set; }
        public long CampaignId { get; set; }
        public long Point { get; set; }

    }
}
