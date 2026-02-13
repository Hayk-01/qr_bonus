using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.BannerService;
using QRBonus.DTO.BannerDtos;
using QRBonus.DTO;

namespace QRBonus.API.Controllers
{
    public class BannerController : ApiControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<PagedResult<List<BannerDto>>> GetAll([FromQuery] BannerFilter filter)
        {
            return await _bannerService.GetAll(filter);
        }

        [HttpGet("{id}")]
        public async Task<Result<BannerDto>> GetById(long id)
        {
            return await _bannerService.GetById(id);
        }

    }
}