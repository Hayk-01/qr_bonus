using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO;
using QRBonus.DTO.ProductDtos;

namespace QRBonus.BLL.Services.ProductService
{
    public interface IProductAdminService
    {
        Task<Result<AddOrUpdateProductDto>> GetByIdAdmin(long id);
        Task<PagedResult<List<ProductDto>>> GetAll(ProductFilter filter);
        Task<Result<BaseDto>> Add(AddOrUpdateProductDto dto);
        Task<Result> Update(long id, AddOrUpdateProductDto dto);
        Task<Result> Delete(long id);
    }
}
