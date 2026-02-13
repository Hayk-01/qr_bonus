using QRBonus.DTO.CampaignDtos;
using QRBonus.DTO.PrizeDtos;

namespace QRBonus.DTO.CustomerDtos
{
    public class CustomerPrizePointDto : BaseDto
    {
        public long CampaignId { get; set; }
        public long Points { get; set; }
        public List<PrizeDto>? Prizes { get; set; }
    }
     public class CustomerRankDto : BaseDto
    {
        public long CustomerId { get; set; }
        public long Point { get; set; }
        public long Rank { get; set; }
       
    }


}
