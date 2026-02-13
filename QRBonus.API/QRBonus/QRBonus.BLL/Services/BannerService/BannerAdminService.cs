using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.BannerDtos;

namespace QRBonus.BLL.Services.BannerService
{
    public class BannerAdminService : BannerBaseService, IBannerAdminService
    {
        private readonly IErrorService _errorService;
        private readonly FileHelper _fileHelper;

        public BannerAdminService(AppDbContext db, FileHelper fileHelper, IErrorService errorService) : base(db)
        {
            _errorService = errorService;
            _fileHelper = fileHelper;

        }

        public async Task<Result<BaseDto>> Add(AddOrUpdateBannerDto dto)
        {
            var banner = new Banner
            {
                ExpirationDate = dto.ExpirationDate,
                Translations = dto.Translations
                .Select(x => new BannerTranslation
                {
                    Name = x.Name,
                    Description = x.Description,
                    LanguageId = x.LanguageId
                }).ToList(),
                RegionId = dto.RegionId,
            };

            _db.Add(banner);
            await _db.SaveChangesAsync();

            if(dto.AddPhoto is not null)
            {
                banner.Link = await _fileHelper.UploadFile(dto.AddPhoto, FileConstants.BannerPhoto, banner.Id.ToString());
                await _db.SaveChangesAsync();
            }

            return new BaseDto
            {
                Id = banner.Id
            };

        }

        public async Task<Result> Delete(long id)
        {
            var banner = await _db.Banners.FirstOrDefaultAsync(x => x.Id == id);    
            if(banner is null)
            {
                return Result.NotFound();
            }

            banner.IsDeleted = true;
            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<AddOrUpdateBannerDto>> GetByIdAdmin(long id)
        {
            var banner = await _db.Banners
                .IgnoreExceptDeleted()
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (banner is null)
            {
                return Result.NotFound();
            }

            var bannerDto = banner.MapToAddOrUpdateBannerDto();

            return bannerDto;
        }

        public async Task<Result> Update(long id, AddOrUpdateBannerDto dto)
        {
            var banner = await _db.Banners
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (banner is null)
            {
                return Result.NotFound();
            }

            banner.ExpirationDate = dto.ExpirationDate;
            banner.ModifyDate = DateTime.UtcNow;

            foreach (var item in dto.Translations)
            {
                var translation = banner.Translations.FirstOrDefault(x => x.LanguageId == item.LanguageId);
                if (translation != null)
                {
                    translation.Name = item.Name;
                    translation.Description = item.Description;
                }
                else
                {
                    translation = new BannerTranslation
                    {
                        Name = item.Name,
                        Description = item.Description,
                        LanguageId = item.LanguageId,
                        BannerId = banner.Id
                    };

                    _db.Add(translation);
                }
            }

            if(dto.AddPhoto is not null)
            {
                banner.Link = await _fileHelper.UploadFile(dto.AddPhoto, FileConstants.BannerPhoto, banner.Id.ToString());
            }

            await _db.SaveChangesAsync();
            return Result.Success();


        }
    }
}