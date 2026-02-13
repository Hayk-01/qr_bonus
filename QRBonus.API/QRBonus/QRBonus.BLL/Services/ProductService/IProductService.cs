using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.ProductDtos;

namespace QRBonus.BLL.Services.ProductService
{
    public interface IProductService
    {
        Task<Result<ProductDto>> GetById(long id);
        Task<PagedResult<List<ProductDto>>> GetAll(ProductFilter filter);

    }
}
