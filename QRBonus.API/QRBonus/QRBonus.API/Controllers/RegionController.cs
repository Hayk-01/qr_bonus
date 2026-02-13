using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.RegionService;
using QRBonus.DTO;
using QRBonus.DTO.RegionDtos;

namespace QRBonus.API.Controllers
{
    public class RegionController : ApiControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<PagedResult<List<RegionDto>>> GetAll([FromQuery] RegionFilter filter)
        {
            return await _regionService.GetAll(filter);
        }
    }
}
