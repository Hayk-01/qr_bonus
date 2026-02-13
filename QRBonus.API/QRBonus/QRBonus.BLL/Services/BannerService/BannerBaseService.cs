using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DTO.BannerDtos;
using QRBonus.DTO.PrizeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.BannerService
{
    public abstract class BannerBaseService
    {
        protected readonly AppDbContext _db;
        public BannerBaseService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<PagedResult<List<BannerDto>>> GetAll(BannerFilter filter)
        {
            var query = _db.Banners
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .AsQueryable();

            var filteredQuery = filter.FilterObjects(query);

            var entities = await filteredQuery
                .ToListAsync();

            List<BannerDto> result = entities.MapToBannerDtos();

            return new PagedResult<List<BannerDto>>(await filter.GetPagedInfoAsync(query), result);

        }
    }
}
