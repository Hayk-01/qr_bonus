using Ardalis.Result;
using InnLine.BLL.Constants;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL.Models;
using QRBonus.DAL;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Mappers;

namespace QRBonus.BLL.Services.PrizeService
{
    public abstract class PrizeBaseService
    {
        protected readonly AppDbContext _db;

        public PrizeBaseService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<PagedResult<List<PrizeDto>>> GetAll(PrizeFilter filter)
        {
            var query = _db.Prizes
                 .Include(x => x.Region)
                 .ThenInclude(x => x.Translations)
                 .Include(x => x.Translations)
                 .AsQueryable();

            var entities = await filter.FilterObjects(query)
                .ToListAsync();

            List<PrizeDto> result = entities.MapToPrizeDtos();

            return new PagedResult<List<PrizeDto>>(await filter.GetPagedInfoAsync(query), result);
        }


    }
}
