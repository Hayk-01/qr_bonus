using QRBonus.BLL.Mappers;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.CampaignDtos;
using QRBonus.DTO.ProductDtos;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class CampaignMapper
{
    public static CampaignDto MapToCampaignDto(this Campaign campaign)
    {
        return new CampaignDto
        {
            Id = campaign.Id,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            Name = campaign.Translations.FirstOrDefault()?.Name,
            Status = campaign.Status,
            ProductPoints = campaign.ProductCampaigns?
            .Select(x => new ProductPointDto
            {
                Point = x.Point,
                Product = x.Product.MapToProductDto()
            }).ToList(),
            ModifyDate = campaign.ModifyDate,
            CreatedDate = campaign.CreatedDate,
            Region = campaign.Region?.MapToRegionDto(),
            RegionId = campaign.RegionId
        };
    }

    public static AddOrUpdateCampaignDto MapToAddOrUpdateCampaignDto(this Campaign campaign)
    {
        return new AddOrUpdateCampaignDto
        {
            Id = campaign.Id,
            StartDate = campaign.StartDate,
            EndDate = campaign.EndDate,
            Status = campaign.Status,
            Translations = campaign.Translations.Select(x =>
            new TranslationDto
            {
                LanguageId = x.LanguageId,
                Name = x.Name
            }).ToList(),

            ProductPoints = campaign.ProductCampaigns?
            .Select(x => new ProductPointDto
            {
                ProductCampaignId = x.Id,
                Point = x.Point,
                Product = x.Product.MapToProductDto(),
                ProductId = x.ProductId
            }).ToList(),
            ModifyDate = campaign.ModifyDate,
            CreatedDate = campaign.CreatedDate,
            Region = campaign.Region?.MapToRegionDto(),
            RegionId = campaign.RegionId
        };
    }

    public static partial List<CampaignDto> MapToCampaignDtos(this List<Campaign> campaigns);

}
