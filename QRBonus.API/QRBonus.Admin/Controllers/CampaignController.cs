using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.ProductService;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO;
using QRBonus.BLL.Services.CampaignService;
using QRBonus.DTO.CampaignDtos;
using QRBonus.Shared.Enums;

namespace QRBonus.Admin.Controllers
{
    public class CampaignController : ApiControllerBase
    {
        public readonly ICampaignAdminService _campaignService;

        public CampaignController(ICampaignAdminService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public async Task<PagedResult<List<CampaignDto>>> GetAll([FromQuery] CampaignFilter filter)
        {
            return await _campaignService.GetAll(filter); ;
        }

        [HttpGet("{id}")]
        public async Task<Result<AddOrUpdateCampaignDto>> GetById(long id)
        {
            return await _campaignService.GetByIdAdmin(id);
        }

        [HttpPost]
        public async Task<Result<BaseDto>> Add([FromBody] AddOrUpdateCampaignDto dto)
        {
            return await _campaignService.Add(dto);
        }

        [HttpPut("{id}")]
        public async Task<Result> Update(long id, [FromBody] AddOrUpdateCampaignDto dto)
        {
            return await _campaignService.Update(id, dto);
        } 
        
        [HttpPut("start/{id}")]
        public async Task<Result> Start(long id)
        {
            return await _campaignService.UpdateStatus(id, (long)CampaignStatus.Active);
        }
        
        [HttpPut("end/{id}")]
        public async Task<Result> End(long id)
        {
            return await _campaignService.UpdateStatus(id, (long)CampaignStatus.Completed);
        }

        //[HttpPut("prolong/{id}")]
        //public async Task<Result> Prolong(long id, [FromBody] DateTime newEndDate)
        //{
        //    return await _campaignService.Prolong(id, newEndDate);
        //}


        [HttpGet("{id}/leaderboard")]
        public async Task<Result<List<LeaderboardDto>>> GetLeaderboard(long id)
        {
            return await _campaignService.GetLeaderboard(id, true);
        }

        [HttpDelete("{id}")]
        public async Task<Result> Delete(long id)
        {
            return await _campaignService.Delete(id);
        }

        [HttpPost("leaderboard/prizes")]
        public async Task<Result> SetPrizes([FromBody] AddOrUpdateCampaignPrizeDto dto)
        {
            return await _campaignService.SetLeaderboardPrizes(dto);
        }

        [HttpGet("{id}/leaderboard/prizes")]
        public async Task<Result<List<PositionPrizeDto>>> GetPrizes(long id)
        {
            return await _campaignService.GetLeaderboardPrizes(id);
        }

    }
}
