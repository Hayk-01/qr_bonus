using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.DAL;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO.RegionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.RegionService
{
    public class RegionBaseService
    {
        protected readonly AppDbContext _db;
        protected readonly LanguageConfiguration _languageConfiguration;

        public RegionBaseService(AppDbContext db, LanguageConfiguration languageConfiguration)
        {
            _db = db;
            _languageConfiguration = languageConfiguration;
        }

        public async Task<PagedResult<List<RegionDto>>> GetAll(RegionFilter filter)
        {
            var query = _db.Regions
                .Include(x => x.Translations)
                .IgnoreExceptDeleted()
                .AsQueryable();

            var entities = await filter.FilterObjects(query)
                .ToListAsync();

            List<RegionDto> result = entities.MapToRegionDtos();

            return new PagedResult<List<RegionDto>>(await filter.GetPagedInfoAsync(query), result);
        }

    }
}
