using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class Banner: BaseWithRegionEntity
    {
        public Banner()
        {
            Translations = new HashSet<BannerTranslation>();
        }
        public string? Link { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public ICollection<BannerTranslation> Translations { get; set; }
    }
}