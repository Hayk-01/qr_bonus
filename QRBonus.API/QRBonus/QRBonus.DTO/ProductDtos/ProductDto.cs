using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.ProductDtos
{
    public class ProductDto : BaseWithRegionDto
    {
        public string Name { get; set; }

        public string? Link { get; set; }
    }
}
