using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class BannerTranslation : BaseTranslationEntity
    {
        public long BannerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public Banner? Banner { get; set; }

    }
}
