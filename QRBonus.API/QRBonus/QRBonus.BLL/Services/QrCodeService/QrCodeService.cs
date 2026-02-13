using Ardalis.Result;
using InnLine.BLL.Constants;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO.QrCodeDtos;
using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.DAL.Models;

namespace QRBonus.BLL.Services.QrCodeService
{
    public class QrCodeService : QrCodeBaseService, IQrCodeService
    {
        private readonly ICustomerSessionService _customerSessionService;

        private readonly List<long> _stupidBlackList = [2056, 567, 684, 11250, 1904, 9031, 601, 41472, 7875];
        private readonly List<long> _whiteListPrizeIds = [28,29];

        public QrCodeService(AppDbContext db, ICustomerSessionService customerSessionService) : base(db)
        {
            _customerSessionService = customerSessionService;
        }

        public async Task<Result<ScannedQrDto>> Scan(string qr)
        {
            var qrCode = await _db.QrCodes
                .Include(x => x.Prize)
                .ThenInclude(x => x.Translations)
                .Include(x => x.ProductCampaign)
                .ThenInclude(x => x.Campaign)
                .FirstOrDefaultAsync(x => x.Value == qr);

            if (qr.Trim() == "http://ma1.am/?q=1UjO3Y603H5cq9Gi")
            {
                var campaign = await _db.Campaigns
                    .Include(x => x.ProductCampaigns)
                    .FirstOrDefaultAsync(x =>
                        x.RegionId == _customerSessionService.CurrentCustomer.RegionId &&
                        x.Status == CampaignStatus.Active);

                if (campaign == null)
                {
                    return Result.Error(ErrorConstants.NotExist.ToString());
                }

                var productCampaign = campaign.ProductCampaigns
                    .Where(x => x.Point <= 2)
                    .OrderByDescending(x => x.Point)
                    .ThenBy(x => x.Id)
                    .FirstOrDefault();

                if (productCampaign == null)
                {
                    return Result.Error(ErrorConstants.NotExist.ToString());
                }

                qr = "http://ma1.am/?q=1UjO3Y603H5cq9Gi" + "_" + _customerSessionService.CurrentCustomer.Id;

                var myCustomQrCode = await _db.QrCodes
                    .FirstOrDefaultAsync(x =>
                        x.Value.StartsWith(qr) && x.CustomerId == _customerSessionService.CurrentCustomer.Id &&
                        x.WinDate != null && x.WinDate > DateTime.UtcNow.AddHours(-48));

                if (myCustomQrCode is not null)
                {
                    return Result.Error(ErrorConstants.AlreadyUsed.ToString());
                }

                qr += "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                _db.QrCodes.Add(new QrCode
                {
                    RegionId = _customerSessionService.CurrentCustomer.RegionId,
                    Value = qr,
                    ActivationDate = DateTime.UtcNow.AddDays(-1),
                    IsExported = true,
                    ProductCampaignId = productCampaign.Id,
                });

                await _db.SaveChangesAsync();

                qrCode = await _db.QrCodes
                    .Include(x => x.Prize)
                    .ThenInclude(x => x.Translations)
                    .Include(x => x.ProductCampaign)
                    .ThenInclude(x => x.Campaign)
                    .FirstOrDefaultAsync(x => x.Value == qr);
            }

            if (qrCode == null || qrCode.ProductCampaign.Campaign.Status == CampaignStatus.Draft)
            {
                return Result.Error(ErrorConstants.NotExist.ToString());
            }

            if (qrCode.CustomerId is not null)
            {
                return Result.Error(ErrorConstants.AlreadyUsed.ToString());
            }

            if (qrCode.RegionId != _customerSessionService.CurrentCustomer.RegionId)
            {
                return Result.Error(ErrorConstants.NotExist.ToString());
            }

            if (qrCode.ProductCampaign.Campaign.Status == CampaignStatus.Completed && qrCode.PrizeId is null)
            {
                return Result.Error(ErrorConstants.TryAgain.ToString());
            }

            if (qrCode.ProductCampaign.Campaign.EndDate < DateTime.UtcNow)
            {
                return Result.Error(ErrorConstants.TemporarilyUnavailable.ToString());
            }

            if (qrCode.ProductCampaign.Campaign.RegionId != 1 &&
                qrCode.ActivationDate >= new DateTime(2025, 12, 10, 19, 59, 59, DateTimeKind.Utc) && //Artaks case
                qrCode.ActivationDate <= new DateTime(2025, 12, 15, 19, 59, 59, DateTimeKind.Utc))
            {
                return Result.Error(ErrorConstants.NotValid.ToString());
            }

            if (qrCode.ActivationDate > DateTime.UtcNow)
            {
                return Result.Invalid(new ValidationError(nameof(qrCode.ActivationDate), $"{qrCode.ActivationDate}"));
            }

            if (_stupidBlackList.Contains(_customerSessionService.CurrentCustomer.Id) && qrCode.PrizeId.HasValue &&
                !_whiteListPrizeIds.Contains(qrCode.PrizeId.Value))
            {
                qrCode.NotGivenPrizeId = qrCode.PrizeId;
                qrCode.PrizeId = null;
            }
            
            if (qrCode.ProductCampaign.Campaign.RegionId != 1 && qrCode.ProductCampaign.Point > 0)
            {
                var zeroProduct = await _db.ProductCampaigns.FirstOrDefaultAsync(x => x.Point == 0 && x.CampaignId == qrCode.ProductCampaign.CampaignId);
                    
                if (zeroProduct != null)
                {
                    qrCode.ProductCampaignId = zeroProduct.Id;
                    qrCode.ProductCampaign = zeroProduct;
                    await _db.SaveChangesAsync();
                }
            }

            qrCode.CustomerId = _customerSessionService.CurrentCustomer.Id;
            qrCode.WinDate = DateTime.UtcNow;
            qrCode.IsWinReceived = !qrCode.PrizeId.HasValue;

            await _db.SaveChangesAsync();

            var qrCodeDto = new ScannedQrDto
            {
                Value = qrCode.Value,
                ProductCampaignId = qrCode.ProductCampaignId,
                Point = qrCode.ProductCampaign.Point,
                PrizeId = qrCode.PrizeId,
                Prize = qrCode.PrizeId.HasValue
                    ? new PrizeDto
                    {
                        Link = qrCode.Prize.Link,
                        Name = qrCode.Prize.Translations.FirstOrDefault()?.Name,
                        IsLeaderboard = qrCode.Prize.IsLeaderboard,
                        ModifyDate = qrCode.Prize.ModifyDate,
                        CreatedDate = qrCode.Prize.CreatedDate,
                    }
                    : null
            };

            return qrCodeDto;
        }

        public async Task<Result<QrHistoryDto>> CustomerHistory(long customerId, string qr)
        {
            var customerQr = await _db.QrCodes
                .Include(x => x.Customer)
                .Include(x => x.Prize)
                .ThenInclude(x => x.Translations)
                .Include(x => x.ProductCampaign)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Value == qr);

            if (customerQr == null)
            {
                return Result.NotFound();
            }

            var qrHistory = new QrHistoryDto();

            qrHistory.ScannedDate = customerQr.WinDate;
            qrHistory.CustomerId = customerQr.CustomerId.Value;
            qrHistory.Value = customerQr.Value;
            qrHistory.IsPrizeReceived = customerQr.IsWinReceived;
            qrHistory.Points = customerQr.ProductCampaign.Point;
            qrHistory.HasCustomerConfirmed = customerQr.HasCustomerConfirmed;
            qrHistory.PrizeDeliveryDate = customerQr.PrizeDeliveryDate;
            qrHistory.PrizeReceiveDate = customerQr.PrizeReceiveDate;
            qrHistory.UserId = customerQr.UserId.Value;

            if (qrHistory.PrizeId is not null)
            {
                qrHistory.PrizeId = customerQr.PrizeId.Value;
                qrHistory.Prize = new PrizeDto
                {
                    Name = customerQr.Prize.Translations.FirstOrDefault().Name,
                    Link = customerQr.Prize.Link,
                    IsLeaderboard = customerQr.Prize.IsLeaderboard,
                    Id = customerQr.PrizeId.Value,
                };
            }

            return qrHistory;
        }

        public async Task<Result> WinReceiveConfirmation(string qr)
        {
            var qrCode = await _db.QrCodes.FirstOrDefaultAsync(x => x.Value == qr);

            if (qrCode is null || qrCode.CustomerId != _customerSessionService.CurrentCustomer.Id ||
                !qrCode.IsWinReceived)
            {
                return Result.NotFound();
            }

            if (qrCode.HasCustomerConfirmed)
            {
                return Result.Error("Already confirmed");
            }

            qrCode.HasCustomerConfirmed = true;
            qrCode.PrizeReceiveDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Result.Success();
        }
    }
}