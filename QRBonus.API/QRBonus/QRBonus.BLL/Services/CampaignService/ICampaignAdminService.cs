using Ardalis.Result;
using QRBonus.DTO.CampaignDtos;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.BLL.Filters;

namespace QRBonus.BLL.Services.CampaignService
{
    public interface ICampaignAdminService
    {
        Task<Result<AddOrUpdateCampaignDto>> GetByIdAdmin(long id);
        Task<PagedResult<List<CampaignDto>>> GetAll(CampaignFilter filter);
        Task<Result<BaseDto>> Add(AddOrUpdateCampaignDto dto);
        Task<Result> Update(long id, AddOrUpdateCampaignDto dto);
        Task<Result> Delete(long id);
        Task<Result> Prolong(long id, DateTime newEndDate);
        Task<Result> UpdateStatus(long id, long statusId);
        Task<Result<List<LeaderboardDto>>> GetLeaderboard(long id, bool isAllNeeded);
        Task<Result> SetLeaderboardPrizes(AddOrUpdateCampaignPrizeDto dto);
        Task<Result<List<PositionPrizeDto>>> GetLeaderboardPrizes(long id);
    }
}
