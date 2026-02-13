using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.BannerDtos;
using QRBonus.DTO.RegionDtos;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class RegionMapper
    {
        public static RegionDto MapToRegionDto(this Region region)
        {
            return new RegionDto
            {
                Id = region.Id,
                CreatedDate = region.CreatedDate,
                ModifyDate = region.ModifyDate,
                Name = region.Translations.FirstOrDefault()?.Name,
                AreaCode = region.AreaCode,
            };
        }

        public static AddOrUpdateRegionDto MapToAddOrUpdateRegionDto(this Region region)
        {
            return new AddOrUpdateRegionDto
            {
                Id = region.Id,
                CreatedDate = region.CreatedDate,
                ModifyDate = region.ModifyDate,
                AreaCode = region.AreaCode,
                Translations = region.Translations.Select(x =>
                new TranslationDto
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                }).ToList(),
            };
        }

        public static partial List<RegionDto> MapToRegionDtos(this List<Region> regions);
    }
}
