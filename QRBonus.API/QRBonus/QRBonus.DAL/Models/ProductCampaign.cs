using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class ProductCampaign :BaseEntity
    {
        public long ProductId {  get; set; }
        public long CampaignId { get; set; }
        public long Point { get; set; }


        public Product? Product { get; set; }
        public Campaign? Campaign { get; set; }
    }
}
