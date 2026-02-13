using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class PrizeTranslation : BaseTranslationEntity
    {
        public long PrizeId {  get; set; }
        public string? Name { get; set; }
        public Prize? Prize { get; set; }
    }
}
