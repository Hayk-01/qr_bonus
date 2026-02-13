using QRBonus.DAL.Models;
using QRBonus.DTO.CampaignDtos;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.DTO.BannerDtos;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class BannerMapper
    {
        public static BannerDto MapToBannerDto(this Banner banner)
        {
            return new BannerDto
            {
                Id = banner.Id,
                CreatedDate = banner.CreatedDate,
                ModifyDate = banner.ModifyDate,
                Name = banner.Translations.FirstOrDefault()?.Name,
                Description = banner.Translations.FirstOrDefault()?.Description,
                Link = banner.Link,
                Region = banner.Region?.MapToRegionDto(),
                RegionId = banner.RegionId
            };
        }

        public static AddOrUpdateBannerDto MapToAddOrUpdateBannerDto(this Banner banner)
        {
            return new AddOrUpdateBannerDto
            {
                Link = banner.Link,
                CreatedDate = banner.CreatedDate,
                ModifyDate = banner.ModifyDate,
                Id = banner.Id,
                ExpirationDate = banner.ExpirationDate,
                Translations = banner.Translations.Select(x =>
                new BannerTranslationDto
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                Region = banner.Region?.MapToRegionDto(),
                RegionId = banner.RegionId
            };
        }

        public static partial List<BannerDto> MapToBannerDtos(this List<Banner> banners);
    }
}
