using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.QrCodeDtos
{
    public class ScannedQrDto
    {
        public string Value { get; set; }
        public long ProductCampaignId { get; set; }
        public long? PrizeId { get; set; }
        public long? Point { get; set; }

        public PrizeDto? Prize { get; set; }
        public ProductDto? Product { get; set; }

    }
}
