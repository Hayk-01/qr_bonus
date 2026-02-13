using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.DAL;
using QRBonus.DTO.ProductDtos;

namespace QRBonus.BLL.Services.ProductService
{
    public class ProductService : ProductBaseService, IProductService
    {
        public ProductService(AppDbContext db) : base(db)
        {
        }

        public async Task<Result<ProductDto>> GetById(long id)
        {
            var product = await _db.Products
                 .Include(x => x.Region)
                 .ThenInclude(x => x.Translations)
                 .Include(x => x.Translations)
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return Result.NotFound();
            }

            var productDto = product.MapToProductDto();

            return productDto;
        }

    }
}
