using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.DAL;
using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO.PrizeDtos;
using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Insights.V1;

namespace QRBonus.BLL.Services.CustomerService
{
    public abstract class CustomerBaseService
    {
        protected readonly AppDbContext _db;
        public CustomerBaseService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<CustomerDto>> GetById(long id)
        {
            var customer = await _db.Customers
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer is null)
            {
                return Result.NotFound();
            }

            CustomerDto customerDto = customer.MapToCustomerDto();

            return customerDto;
        }

        public async Task<PagedResult<List<CustomerDto>>> GetAll(CustomerFilter filter)
        {
            var query = _db.Customers
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .AsQueryable();

            var entities = await filter.FilterObjects(query.AsSplitQuery()).ToListAsync();



            return new PagedResult<List<CustomerDto>>(await filter.GetPagedInfoAsync(query), entities.MapToCustomerDtos());
        }

        public async Task<Result<List<CustomerPrizePointDto>>> GetPrizesAndPoints(long id)
        {
            var customerPrizesPointsPerCampaign = await _db.QrCodes
                .Where(x => x.CustomerId == id)
                .Include(x => x.ProductCampaign)
                .GroupBy(x => x.ProductCampaign.CampaignId)
                .Select(x => new CustomerPrizePointDto
                {
                    CampaignId = x.Key,
                    Points = x.Sum(qr => qr.ProductCampaign.Point)
                })
                .ToListAsync();
            
            var customerPrizesPerCampaign = await _db.QrCodes
                .Where(x => x.CustomerId == id && x.PrizeId != null)
                .Select(x => new
                {
                    CampaignId = x.ProductCampaign.CampaignId,
                    PrizeName = x.Prize.Translations.FirstOrDefault().Name,
                    PrizeLink = x.Prize.Link,
                    PrizeId = x.Prize.Id,
                    IsLeaderboard = x.Prize.IsLeaderboard,
                })
                .ToListAsync();

            var grouppedCustomerPrizesPerCampaign = customerPrizesPerCampaign.GroupBy(x => x.CampaignId);

            foreach(var gr in grouppedCustomerPrizesPerCampaign)
            {
                var campaign = customerPrizesPointsPerCampaign.FirstOrDefault(x => x.CampaignId == gr.Key);

                if(campaign != null)
                {
                    campaign.Prizes = gr.Select(x => new PrizeDto
                    {
                        Link = x.PrizeLink,
                        Name = x.PrizeName,
                        IsLeaderboard = x.IsLeaderboard
                    }).ToList();
                }
                else
                {
                    campaign = new CustomerPrizePointDto
                    {
                        CampaignId = gr.Key,
                        Prizes = gr.Select(x => new PrizeDto
                        {
                            Link = x.PrizeLink,
                            Name = x.PrizeName,
                            IsLeaderboard = x.IsLeaderboard
                        }).ToList()
                    };

                    customerPrizesPointsPerCampaign.Add(campaign);    
                }
            }
            return customerPrizesPointsPerCampaign;
        }
        
        public async Task<Result<CustomerRankDto>> GetRank(long id)
        {
            var campaign = await _db.Campaigns.FirstOrDefaultAsync(x => x.Status == CampaignStatus.Active);

            if (campaign is null)
            {
                return Result.Error("There is no any active campaign"); //TODO: error
            }

            var customerPoint = await _db.QrCodes
                .Where(x => x.CustomerId != null && x.ProductCampaign.CampaignId == campaign.Id)
                .Include(x => x.ProductCampaign)
                .GroupBy(x => x.CustomerId)
                .Select(x => new
                {
                    CustomerId = x.Key,
                    Point = x.Sum(y => y.ProductCampaign.Point),
                    WinDate = x.Max(y => y.WinDate)
                })
                .OrderByDescending(x => x.Point)
                .ThenBy(x => x.WinDate)
                .ToListAsync();

            var rank = customerPoint.FirstOrDefault(x => x.CustomerId == id);

            return new CustomerRankDto
            {
                CustomerId = id,
                Point = rank?.Point ?? 0,
                Rank = customerPoint.IndexOf(rank) + 1
            };

        }
    }
}
