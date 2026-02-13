using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Mappers;
using QRBonus.DAL;
using QRBonus.DTO.BannerDtos;
namespace QRBonus.BLL.Services.BannerService
{
    public class BannerService : BannerBaseService, IBannerService
    {
        public BannerService(AppDbContext db) : base(db)
        {

        }

        public async Task<Result<BannerDto>> GetById(long id)
        {
            var banner = await _db.Banners
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (banner is null)
            {
                return Result.NotFound();
            }
            
            BannerDto dto = banner.MapToBannerDto();

            return dto;
        }
    }
}
