using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.PrizeDtos;
using Riok.Mapperly.Abstractions;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class PrizeMapper
    {
        public static PrizeDto MapToPrizeDto(this Prize prize)
        {
            return new PrizeDto
            {
                IsLeaderboard = prize.IsLeaderboard,
                Name = prize.Translations.FirstOrDefault()?.Name,
                Link = prize.Link,
                Id = prize.Id,
                CreatedDate = prize.CreatedDate,
                ModifyDate = prize.ModifyDate,
                Region = prize.Region?.MapToRegionDto(),
                RegionId = prize.RegionId
            };
        }
        public static AddOrUpdatePrizeDto MapToAddOrUpdatePrizeDto(this Prize prize)
        {
            return new AddOrUpdatePrizeDto
            {
                IsLeaderboard = prize.IsLeaderboard,
                Translations = prize.Translations
                .Select(x => new TranslationDto
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name
                }).ToList(),
                Link = prize.Link,
                Id = prize.Id,
                CreatedDate = prize.CreatedDate,
                ModifyDate = prize.ModifyDate,
                Region = prize.Region?.MapToRegionDto(),
                RegionId = prize.RegionId
            };
        }

        public static partial List<PrizeDto> MapToPrizeDtos(this List<Prize> prizes);
    }
}
