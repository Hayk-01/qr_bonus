using QRBonus.DTO.RegionDtos;

namespace QRBonus.DTO;

public class BaseWithRegionDto : BaseDto
{
    public long RegionId { get; set; }

    public RegionDto? Region { get; set; }
}