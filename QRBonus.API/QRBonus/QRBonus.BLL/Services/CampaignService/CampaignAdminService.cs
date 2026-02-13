using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO;
using QRBonus.DTO.CampaignDtos;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using InnLine.BLL.Constants;
using Microsoft.EntityFrameworkCore;
using QRBonus.DAL.Models;
using QRBonus.Shared.Enums;
using QRBonus.BLL.Helpers;
using Twilio.Rest.Assistants.V1;
using QRBonus.BLL.Mappers;

namespace QRBonus.BLL.Services.CampaignService
{
    public class CampaignAdminService : CampaignBaseService, ICampaignAdminService
    {
        private readonly LanguageConfiguration _languageConfiguration;
        public CampaignAdminService(AppDbContext db, IErrorService errorService, LanguageConfiguration languageConfiguration) : base(db, errorService)
        {
            _languageConfiguration = languageConfiguration;
        }

        public async Task<Result<AddOrUpdateCampaignDto>> GetByIdAdmin(long id)
        {
            var campaign = await _db.Campaigns
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .Include(x => x.Region)
                .ThenInclude(x => x.Translations)
                .Include(x => x.ProductCampaigns)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Translations.OrderByDescending(x => x.LanguageId == _languageConfiguration.LanguageId))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return Result.NotFound();
            }

            var campaignDto = campaign.MapToAddOrUpdateCampaignDto();

            return campaignDto;
        }

        public async Task<Result<BaseDto>> Add(AddOrUpdateCampaignDto dto)
        {
            if (dto.StartDate > dto.EndDate)
            {
                return Result.Error();
            }

            var campaign = new Campaign
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = CampaignStatus.Draft,
                ProductCampaigns = dto.ProductPoints?.Select(x => new ProductCampaign
                {
                    ProductId = x.ProductId,
                    Point = x.Point,
                }).ToList() ?? [],
                Translations = dto.Translations.Select(x => new CampaignTranslation
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                }).ToList(),
                RegionId = dto.RegionId
            };

            _db.Campaigns.Add(campaign);
            await _db.SaveChangesAsync();

            return new BaseDto
            {
                Id = campaign.Id
            };
        }

        public async Task<Result> Prolong(long id, DateTime newEndDate)
        {
            var campaign = await _db.Campaigns
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null || campaign.Status == CampaignStatus.Completed)
            {
                return Result.NotFound();
            }

            if (campaign.StartDate > newEndDate || newEndDate < DateTime.Today)
            {
                return Result.Error();
            }

            campaign.Status = CampaignStatus.Active;
            campaign.EndDate = newEndDate;

            await _db.SaveChangesAsync();

            return Result.Success();

        }

        public async Task<Result> Update(long id, AddOrUpdateCampaignDto dto)
        {
            var campaign = await _db.Campaigns
                .IgnoreExceptDeleted()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return Result.NotFound();
            }

            if (dto.StartDate > dto.EndDate)
            {
                return Result.Error();
            }

            campaign.StartDate = dto.StartDate;
            campaign.EndDate = dto.EndDate;

            foreach (var item in dto.Translations) 
            {
                var translation = campaign.Translations.FirstOrDefault(x => x.LanguageId == item.LanguageId);
                if (translation != null) 
                {
                    translation.Name = item.Name;
                }
                else
                {
                    translation = new CampaignTranslation
                    {
                        Name = item.Name,
                        LanguageId = item.LanguageId,
                        CampaignId = campaign.Id
                    };

                    _db.Add(translation);
                }
            }

            if (dto.ProductPoints is not null)
            {
                await UpdateProductList(campaign, dto.ProductPoints);
            }

            await _db.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateStatus(long id, long statusId)
        {
            var campaign = await _db.Campaigns
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return Result.NotFound();
            }

            if (statusId == (long)CampaignStatus.Active)
            {
                var hasProduct = await _db.ProductCampaigns
                    .AnyAsync(x => x.CampaignId == campaign.Id);
                
                var isAnyCampaignStarted = await _db.Campaigns.AnyAsync(x => x.Status == CampaignStatus.Active && x.RegionId == campaign.RegionId);

                if (campaign.StartDate >= campaign.EndDate || !hasProduct || isAnyCampaignStarted)
                {
                    return Result.Error();
                }
                campaign.Status = CampaignStatus.Active;
            }

            if (statusId == (long)CampaignStatus.Completed)
            {
                if (campaign.Status != CampaignStatus.Active)
                {
                    return Result.NotFound();
                }
                campaign.Status = CampaignStatus.Completed;
            }

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        private async Task<Result> UpdateProductList(Campaign campaign, List<ProductPointDto> productPoints)
        {
            var currentProductCampaigns = await _db.ProductCampaigns
                .Where(x => x.CampaignId == campaign.Id)
                .ToListAsync();

            //add
            foreach (var productPoint in productPoints)
            {
                if (!currentProductCampaigns!.Select(x => x.ProductId).Contains(productPoint.ProductId))
                {
                    _db.ProductCampaigns.Add(new ProductCampaign
                    {
                        ProductId = productPoint.ProductId,
                        CampaignId = campaign.Id,
                        Point = productPoint.Point
                    });

                }
                else
                {
                    currentProductCampaigns.FirstOrDefault(x => x.ProductId == productPoint.ProductId).Point = productPoint.Point;
                }

            }

            //delete
            foreach (var currentProductCampaign in currentProductCampaigns)
            {   
                if (!productPoints.Select(x => x.ProductId)
                                  .Contains(currentProductCampaign.ProductId))
                {
                    if (campaign.Status != CampaignStatus.Draft)
                    {
                        return Result.Error("You can't delete products from active campaigns. Please change status to draft and try again.");
                    }

                    var currentQrCodes = await _db.QrCodes
                        .Where(x => x.ProductCampaignId == currentProductCampaign.Id)
                        .ToListAsync();
                    
                    _db.QrCodes.RemoveRange(currentQrCodes);
                    _db.ProductCampaigns.Remove(currentProductCampaign);
                }
            }
            
            return Result.Success();
        }

        public async Task<Result> Delete(long id)
        {
            var campaign = await _db.Campaigns
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null || campaign.Status != CampaignStatus.Draft)
            {
                return Result.NotFound();
            }

            campaign.IsDeleted = true;
            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> SetLeaderboardPrizes(AddOrUpdateCampaignPrizeDto dto)
        {
            if (await _db.Prizes.AnyAsync(x => x.IsLeaderboard == false && dto.PrizesAsc.Contains(x.Id)))
            {
                return Result.Error();     //ToDo: add error service
            }

            var currentPrizes = await _db.CampaignPrizes.Where(x => x.CampaignId == dto.CampaignId).ToListAsync();

            _db.CampaignPrizes.RemoveRange(currentPrizes);

            int quantity = dto.PrizesAsc.Length;

            for (int i = 1; i <= quantity; i++)
            {
                _db.CampaignPrizes.Add(new CampaignPrize
                {
                    Position = i,
                    PrizeId = dto.PrizesAsc[i - 1],
                    CampaignId = dto.CampaignId,
                });
            }

            await _db.SaveChangesAsync();

            return Result.Success();
        }
    }
}