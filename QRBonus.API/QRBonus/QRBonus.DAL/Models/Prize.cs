using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class Prize : BaseWithRegionEntity
    {
        public Prize()
        {
            Translations = new HashSet<PrizeTranslation>();
        }
        public bool IsLeaderboard { get; set; }
        public string? Link { get; set; }

        public ICollection<PrizeTranslation> Translations { get; set; }
    }
}
