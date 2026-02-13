using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.RegionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.RegionService
{
    public class RegionAdminService : RegionBaseService, IRegionAdminService
    {
        private readonly LanguageConfiguration _languageConfiguration;

        public RegionAdminService(AppDbContext db, LanguageConfiguration languageConfiguration) : base(db, languageConfiguration)
        {
        }

        public async Task<Result<BaseDto>> Add(AddOrUpdateRegionDto dto)
        {
            if ((await _db.Regions.FirstOrDefaultAsync(x => x.AreaCode == dto.AreaCode) is not null))
            {
                return Result.Error("Duplicate");
            }

            var region = new Region
            {
                Translations = dto.Translations.Select(x => new RegionTranslation
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                }).ToList(),

                AreaCode = dto.AreaCode,
            };

            _db.Regions.Add(region);

            await _db.SaveChangesAsync();

            return new BaseDto
            {
                Id = region.Id,
            };
        }

        public async Task<Result> Delete(long id)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region is null)
            {
                return Result.NotFound();
            }

            region.IsDeleted = true;

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<AddOrUpdateRegionDto>> GetByIdAdmin(long id)
        {
            var region = await _db.Regions
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (region is null)
            {
                return Result.NotFound();
            }

            var result = region.MapToAddOrUpdateRegionDto();

            return result;
        }

        public async Task<Result> Update(long id, AddOrUpdateRegionDto dto)
        {
            var region = await _db.Regions
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (region is null)
            {
                return Result.NotFound();
            }

            region.ModifyDate = DateTime.UtcNow;

            foreach (var item in dto.Translations)
            {
                var translation = region.Translations.FirstOrDefault(x => x.LanguageId == item.LanguageId);
                if (translation != null)
                {
                    translation.Name = item.Name;
                }
                else
                {
                    translation = new RegionTranslation
                    {
                        Name = item.Name,
                        LanguageId = item.LanguageId,
                        RegionId = region.Id
                    };

                    _db.Add(translation);
                }
            }

            await _db.SaveChangesAsync();

            return Result.Success();

        }
    }
}
