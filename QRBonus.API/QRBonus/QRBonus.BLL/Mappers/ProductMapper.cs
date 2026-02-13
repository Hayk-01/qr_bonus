using QRBonus.BLL.Mappers;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.ProductDtos;
using Riok.Mapperly.Abstractions;


[Mapper]
public static partial class ProductMapper
{
    public static ProductDto MapToProductDto(this Product product)
    {
        return new ProductDto
        {
            Link = product.Link,
            CreatedDate = product.CreatedDate,
            Id = product.Id,
            ModifyDate = product.ModifyDate,
            Name = product.Translations.FirstOrDefault().Name,
            Region = product.Region?.MapToRegionDto(),
            RegionId = product.RegionId          
        };
    }
     public static AddOrUpdateProductDto MapToAddOrUpdateProductDto(this Product product)
    {
        return new AddOrUpdateProductDto
        {
            Link = product.Link,
            CreatedDate = product.CreatedDate,
            Id = product.Id,
            ModifyDate = product.ModifyDate,
            Translations = product.Translations.Select(x => new TranslationDto
            {
                Name = x.Name,
                LanguageId = x.LanguageId,
            }).ToList(),
            Region = product.Region?.MapToRegionDto(),
            RegionId = product.RegionId
        };
    }

    public static partial List<ProductDto> MapToProductDtos(this List<Product> products);

}
