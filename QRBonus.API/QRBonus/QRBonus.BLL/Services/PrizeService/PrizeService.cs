using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DTO.PrizeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.PrizeService
{
    public class PrizeService: PrizeBaseService, IPrizeService
    {
        public PrizeService(AppDbContext db) : base(db)
        {

        }

        public async Task<Result<PrizeDto>> GetById(long id)
        {
            var prize = await _db.Prizes
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (prize is null)
            {
                return Result.NotFound();
            }

            PrizeDto dto = prize.MapToPrizeDto();
            return dto;
        }

    }
}
