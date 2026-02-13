using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.QrCodeDtos
{
    public class AddQrCodeDto
    {
        public long ProductCampaignId { get; set; }
        public List<AddQrCodeCountDto> QrCodeCounts {  get; set; } 
        public DateTime ActivationDate { get; set; }
    }
    
    public class AddQrCodeCountDto
    {
        public long? PrizeId { get; set; }
        public int Count { get; set; }
 
    }
}
