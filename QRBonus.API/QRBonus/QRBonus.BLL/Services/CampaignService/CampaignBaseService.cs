using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO.CampaignDtos;
using QRBonus.DTO.PrizeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Insights.V1;

namespace QRBonus.BLL.Services.CampaignService
{
    public abstract class CampaignBaseService
    {
        protected readonly AppDbContext _db;
        protected readonly IErrorService _errorService;

        public CampaignBaseService(AppDbContext db, IErrorService errorService)
        {
            _db = db;
            _errorService = errorService;
        }

        public async Task<PagedResult<List<CampaignDto>>> GetAll(CampaignFilter filter)
        {
            var query = _db.Campaigns
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations)
                .AsQueryable();

            var entities = await filter.FilterObjects(query)
                .ToListAsync();

            List<CampaignDto> result = entities.MapToCampaignDtos();

            return new PagedResult<List<CampaignDto>>(await filter.GetPagedInfoAsync(query), result);
        }

        public async Task<Result<List<PositionPrizeDto>>> GetLeaderboardPrizes(long id)
        {
            var campaignPrizes = await _db.CampaignPrizes
                .Include(x => x.Prize)
                .ThenInclude(x => x.Translations)
                .Where(x => x.CampaignId == id && x.Prize.IsLeaderboard)
                .Select(x => new PositionPrizeDto
                {
                    Position = x.Position,
                    Prize = new PrizeDto
                    {
                        Id = x.PrizeId,
                        Link = x.Prize.Link,
                        Name = x.Prize.Translations.FirstOrDefault().Name,
                        IsLeaderboard = x.Prize.IsLeaderboard,
                    }
                }).ToListAsync();

            return campaignPrizes;
        }

        public virtual async Task<Result<List<LeaderboardDto>>> GetLeaderboard(long campaignId, bool isAllNeeded = false)
        {
            var qrCodes = await _db.QrCodes
                .Include(x => x.ProductCampaign)
                .Where(x => x.CustomerId != null && x.ProductCampaign.CampaignId == campaignId)
                .GroupBy(x => x.CustomerId)
                .Select(x => new
                {
                    CustomerId = x.Key,
                    Points = x.Sum(y => y.ProductCampaign.Point),
                    WinDate = x.Max(y => y.WinDate)
                })
                .OrderByDescending(x => x.Points)
                .ThenBy(x => x.WinDate)
                .Take(isAllNeeded ? int.MaxValue : 10)
                .ToListAsync();


            var lst = new List<LeaderboardDto>();

            var position = 1;

            var customers = await _db.Customers
                .Where(x => qrCodes
                .Select(y => y.CustomerId)
                .Contains(x.Id))
                .ToListAsync();

            foreach (var c in qrCodes)
            {
                lst.Add(new LeaderboardDto
                {
                    Points = c.Points,
                    Position = position,
                    Customer = customers.FirstOrDefault(x => x.Id == c.CustomerId)?.MapToCustomerDto()
                });
                position++;
            }

            return lst;
        }
    }
}