using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Mappers;
using QRBonus.DAL;
using QRBonus.DTO.RegionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.RegionService
{
    public class RegionService : RegionBaseService, IRegionService
    {
        public RegionService(AppDbContext db, LanguageConfiguration languageConfiguration) : base(db, languageConfiguration)
        {

        }
        public async Task<Result<RegionDto>> GetById(long id)
        {
            var region = await _db.Regions.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);

            if (region is null)
            {
                return Result.NotFound();
            }

            return region.MapToRegionDto();
        }
    }
}