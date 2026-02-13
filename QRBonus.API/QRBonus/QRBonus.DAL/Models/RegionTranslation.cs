using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class RegionTranslation : BaseTranslationEntity
    {
        public long RegionId { get; set; }
        public string Name { get; set; }

        public Region? Region { get; set; }
    }
}
