using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.PrizeService;
using QRBonus.DTO;
using QRBonus.DTO.PrizeDtos;

namespace QRBonus.Admin.Controllers
{

    public class PrizeController : ApiControllerBase
    {
        private readonly IPrizeAdminService _prizeAdminService;

        public PrizeController(IPrizeAdminService prizeAdminService)
        {
            _prizeAdminService = prizeAdminService;
        }

        [HttpPost]
        public async Task<Result<BaseDto>> Add ([FromBody] AddOrUpdatePrizeDto dto)
        {
            return await _prizeAdminService.Add(dto);
        }

        [HttpGet]
        public async Task<PagedResult<List<PrizeDto>>> GetAll([FromQuery] PrizeFilter filter)
        {
            return await _prizeAdminService.GetAll(filter); 
        }

        [HttpGet("{id}")]
        public async Task<Result<AddOrUpdatePrizeDto>> GetById(long id)
        {
            return await _prizeAdminService.GetByIdAdmin(id);
        }

        [HttpPut("{id}")]
        public async Task<Result> Update (long id, [FromBody] AddOrUpdatePrizeDto dto)
        {
            return await _prizeAdminService.Update(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<Result> Delete (long id)
        {
            return await _prizeAdminService.Delete(id);
        }
       

    }
}
