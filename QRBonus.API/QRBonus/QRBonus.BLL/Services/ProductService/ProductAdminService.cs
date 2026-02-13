using Ardalis.Result;
using InnLine.BLL.Constants;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.ProductService
{
    public class ProductAdminService : ProductBaseService, IProductAdminService
    {
        private readonly IErrorService _errorService;
        private readonly FileHelper _fileHelper;

        public ProductAdminService(AppDbContext db, IErrorService errorService, FileHelper fileHelper) : base(db)
        {
            _errorService = errorService;
            _fileHelper = fileHelper;
        }

        public async Task<Result<AddOrUpdateProductDto>> GetByIdAdmin(long id)
        {
            var product = await _db.Products
                 .IgnoreExceptDeleted()
                 .Include(x => x.Region)
                 .ThenInclude(x => x.Translations)
                 .Include(x => x.Translations)
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return Result.NotFound();
            }

            var productDto = product.MapToAddOrUpdateProductDto();

            return productDto;
        }

        public async Task<Result<BaseDto>> Add(AddOrUpdateProductDto dto)
        {
            var product = new Product
            {
                Translations = dto.Translations.Select(x => new ProductTranslation
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                }).ToList(),
                RegionId = dto.RegionId,
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            if (dto.AddPhoto is not null)
            {
                product.Link = await _fileHelper.UploadFile(dto.AddPhoto, FileConstants.ProductPhoto, product.Id.ToString());
                await _db.SaveChangesAsync();
            }

            return new BaseDto
            {
                Id = product.Id
            };
        }

        public async Task<Result> Delete(long id)
        {
            var product = await _db.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return Result.NotFound();
            }

            product.IsDeleted = true;
            await _db.SaveChangesAsync();

            return Result.Success();
        }


        public async Task<Result> Update(long id, AddOrUpdateProductDto dto)
        {
            var product = await _db.Products
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return Result.NotFound();
            }

            product.ModifyDate = DateTime.UtcNow;

            foreach (var item in dto.Translations)
            {
                var translation = product.Translations.FirstOrDefault(x => x.LanguageId == item.LanguageId);
                if (translation != null)
                {
                    translation.Name = item.Name;
                }
                else
                {
                    translation = new ProductTranslation
                    {
                        Name = item.Name,
                        LanguageId = item.LanguageId,
                        ProductId = product.Id
                    };

                    _db.Add(translation);
                }
            }

            if (dto.AddPhoto is not null)
            {
                product.Link = await _fileHelper.UploadFile(dto.AddPhoto, FileConstants.ProductPhoto, product.Id.ToString());
            }

            await _db.SaveChangesAsync();
            return Result.Success();
        }
    }
}
