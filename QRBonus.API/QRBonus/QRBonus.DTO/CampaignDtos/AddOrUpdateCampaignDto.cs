using QRBonus.DTO.ProductDtos;
using QRBonus.Shared.Enums;

namespace QRBonus.DTO.CampaignDtos
{
    public class AddOrUpdateCampaignDto : BaseWithRegionDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CampaignStatus? Status { get; set; }

        public List<ProductPointDto>? ProductPoints { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
