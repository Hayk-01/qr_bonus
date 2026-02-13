using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class Region : BaseEntity
    {
        public Region()
        {
            Translations = new HashSet<RegionTranslation>();
        }
        public string AreaCode { get; set; }

        public ICollection<RegionTranslation> Translations { get; set; }
    }
}
