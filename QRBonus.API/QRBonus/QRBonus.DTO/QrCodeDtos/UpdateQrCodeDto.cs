using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.QrCodeDtos
{
    public class UpdateQrCodeDto : BaseDto
    {
        public string Value { get; set; }
        public long ProductCampaignId { get; set; }
        public long? PrizeId { get; set; }
        public long? CustomerId { get; set; }

    }
}
