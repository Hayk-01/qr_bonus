using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.CampaignService;
using QRBonus.DAL.Models;
using QRBonus.DTO.CampaignDtos;

namespace QRBonus.API.Controllers
{
    public class CampaignController : ApiControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public async Task<PagedResult<List<CampaignDto>>> GetAll([FromQuery] CampaignFilter filter)
        {
            return await _campaignService.GetAll(filter); ;
        }

        [HttpGet("{id}/leaderboard")]
        public async Task<Result<List<LeaderboardDto>>> GetLeaderboard(long id)
        {
            return await _campaignService.GetLeaderboard(id);
        }

        [HttpGet("{id}/leaderboard/prizes")]
        public async Task<Result<List<PositionPrizeDto>>> GetLeaderboardPrizes(long id)
        {
            return await _campaignService.GetLeaderboardPrizes(id);
        }

        [HttpGet("{id}")]
        public async Task<Result<CampaignDto>> GetById(long id)
        {
            return await _campaignService.GetById(id);
        }
    }
}
