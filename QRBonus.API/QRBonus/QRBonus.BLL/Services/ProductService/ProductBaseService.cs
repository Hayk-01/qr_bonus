using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.DAL;
using QRBonus.DTO.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.ProductService
{
    public abstract class ProductBaseService
    {
        protected readonly AppDbContext _db;
        public ProductBaseService(AppDbContext db)
        {
             _db = db;
        }

        public async Task<PagedResult<List<ProductDto>>> GetAll(ProductFilter filter)
        {
            var query = _db.Products
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .AsQueryable();

            var entities = await filter.FilterObjects(query)
                .ToListAsync();

            List<ProductDto> result = entities.MapToProductDtos();

            return new PagedResult<List<ProductDto>>(await filter.GetPagedInfoAsync(query), result);
        }


    }
}
