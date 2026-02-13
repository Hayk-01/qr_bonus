using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.CampaignDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<Result<CampaignDto>> GetById(long id);
        Task<PagedResult<List<CampaignDto>>> GetAll(CampaignFilter filter);
        Task<Result<List<LeaderboardDto>>> GetLeaderboard(long id);
        Task<Result<List<PositionPrizeDto>>> GetLeaderboardPrizes(long id);
    }
}
