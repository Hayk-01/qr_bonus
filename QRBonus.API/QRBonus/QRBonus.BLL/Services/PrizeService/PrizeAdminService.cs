using Ardalis.Result;
using InnLine.BLL.Constants;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.PrizeService
{
    public class PrizeAdminService : PrizeBaseService, IPrizeAdminService
    {
        private readonly IErrorService _errorService;
        private readonly FileHelper _fileHelper;

        public PrizeAdminService(AppDbContext db, FileHelper fileHelper, IErrorService errorService) : base(db)
        {
            _errorService = errorService;
            _fileHelper = fileHelper;     
        }

        public async Task<Result<BaseDto>> Add(AddOrUpdatePrizeDto dto)
        {
            var prize = new Prize
            {
                IsLeaderboard = dto.IsLeaderboard,
                Translations = dto.Translations
                .Select(x => new PrizeTranslation
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                }).ToList(),
                RegionId = dto.RegionId,
            };

            _db.Prizes.Add(prize);
            await _db.SaveChangesAsync();

            if (dto.AddPhoto is not null)
            {
                prize.Link = await _fileHelper.UploadFile(dto.AddPhoto, FileConstants.PrizePhoto, prize.Id.ToString());
                await _db.SaveChangesAsync();
            }

            return new BaseDto
            {
                Id = prize.Id
            };
        }

        public async Task<Result<AddOrUpdatePrizeDto>> GetByIdAdmin(long id)
        {
            var prize = await _db.Prizes
                .IgnoreExceptDeleted()
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (prize is null)
            {
                return Result.NotFound();
            }

            var dto = prize.MapToAddOrUpdatePrizeDto();
            return dto;
        }


        public async Task<Result> Delete(long id)
        {
            var prize =await _db.Prizes.FirstOrDefaultAsync(x => x.Id == id);

            if (prize is null)
            {
                return Result.NotFound();
            }

            prize.IsDeleted = true;

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Update(long id, AddOrUpdatePrizeDto dto)
        {
            var prize = await _db.Prizes
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (prize is null)
            {
                return Result.NotFound();
            }

            //prize.Name = dto.Name; //TODO: Gaco ba vor arden shahel en?

            prize.ModifyDate = DateTime.UtcNow;

            foreach (var item in dto.Translations)
            {
                var translation = prize.Translations.FirstOrDefault(x => x.LanguageId == item.LanguageId);
                if (translation != null)
                {
                    translation.Name = item.Name;
                }
                else
                {
                    translation = new PrizeTranslation
                    {
                        Name = item.Name,
                        LanguageId = item.LanguageId,
                        PrizeId = prize.Id
                    };

                    _db.Add(translation);
                }
            }

            if (dto.AddPhoto is not null)
            {
                prize.Link = await _fileHelper.UploadFile(dto.AddPhoto, FileConstants.PrizePhoto, prize.Id.ToString());
            }

            await _db.SaveChangesAsync();
            return Result.Success();
        }
    }
}
