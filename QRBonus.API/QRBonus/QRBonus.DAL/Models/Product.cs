using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class Product : BaseWithRegionEntity
    {
        public Product()
        {
            Translations = new HashSet<ProductTranslation>();
        }
        public string? Link { get; set; }

        public ICollection<ProductTranslation> Translations { get; set; }
    }
}
