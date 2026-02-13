using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DTO.CampaignDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.CampaignService
{
    public class CampaignService : CampaignBaseService, ICampaignService
    {
        private readonly ICustomerSessionService _customerSessionService;
        private readonly ICustomerService _customerService;
        public CampaignService(AppDbContext db, IErrorService errorService, ICustomerSessionService customerSessionService, ICustomerService customerService) : base(db, errorService)
        {
            _customerSessionService = customerSessionService;
            _customerService = customerService;
        }

        public async Task<Result<CampaignDto>> GetById(long id)
        {
            var campaign = await _db.Campaigns
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.ProductCampaigns)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return Result.NotFound();
            }

            var campaignDto = campaign.MapToCampaignDto();

            return campaignDto;
        }

        public async Task<Result<List<LeaderboardDto>>> GetLeaderboard(long campaignId)
        {
            var result = await base.GetLeaderboard(campaignId);

            var currentCustomer = _customerSessionService.CurrentCustomer;

            if(result.Value.Count == 10 && currentCustomer != null 
                && !result.Value.Any(x => x.Customer != null && x.Customer.Id == currentCustomer.Id))
            {
                var currentCustomerRank = await _customerService.GetRank(currentCustomer.Id);

                if(currentCustomerRank.Value.Rank > 10)
                {
                    result.Value.Add(new LeaderboardDto
                    {
                        Points = currentCustomerRank.Value.Point,
                        Position = currentCustomerRank.Value.Rank,
                        Customer = currentCustomer.MapToCustomerDto(),
                    });

                }
            }
            return result;
        }
    }
}