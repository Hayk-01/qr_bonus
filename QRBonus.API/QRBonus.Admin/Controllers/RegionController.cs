using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.DTO;
using QRBonus.BLL.Services.RegionService;
using QRBonus.DTO.RegionDtos;

namespace QRBonus.Admin.Controllers
{
    public class RegionController : ApiControllerBase
    {
        private readonly IRegionAdminService _regionAdminService;

        public RegionController(IRegionAdminService regionAdminService)
        {
            _regionAdminService = regionAdminService;
        }

        [HttpPost]
        public async Task<Result<BaseDto>> Add([FromBody] AddOrUpdateRegionDto dto)
        {
            return await _regionAdminService.Add(dto);
        }

        [HttpGet]
        public async Task<PagedResult<List<RegionDto>>> GetAll([FromQuery] RegionFilter filter)
        {
            return await _regionAdminService.GetAll(filter);
        }

        [HttpGet("{id}")]
        public async Task<Result<AddOrUpdateRegionDto>> GetById(long id)
        {
            return await _regionAdminService.GetByIdAdmin(id);
        }
    }
}