using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.BannerService;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO;
using QRBonus.DTO.BannerDtos;

namespace QRBonus.Admin.Controllers
{
    public class BannerController : ApiControllerBase
    {
        private readonly IBannerAdminService _bannerAdminService;

        public BannerController(IBannerAdminService bannerAdminService)
        {
            _bannerAdminService = bannerAdminService;
        }

        [HttpPost]
        public async Task<Result<BaseDto>> Add([FromBody] AddOrUpdateBannerDto dto)
        {
            return await _bannerAdminService.Add(dto);
        }

        [HttpGet]
        public async Task<PagedResult<List<BannerDto>>> GetAll([FromQuery] BannerFilter filter)
        {
            return await _bannerAdminService.GetAll(filter);
        }

        [HttpGet("{id}")]
        public async Task<Result<AddOrUpdateBannerDto>> GetById(long id)
        {
            return await _bannerAdminService.GetByIdAdmin(id);
        }

        [HttpPut("{id}")]
        public async Task<Result> Update(long id, [FromBody] AddOrUpdateBannerDto dto)
        {
            return await _bannerAdminService.Update(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<Result> Delete(long id)
        {
            return await _bannerAdminService.Delete(id);
        }

    }
}
