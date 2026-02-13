using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO.QrCodeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.QrCodeService
{
    public abstract class QrCodeBaseService
    {
        protected readonly AppDbContext _db;
        public QrCodeBaseService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<List<QrHistoryDto>>> CustomerHistory(long id)
        {
            List<QrHistoryDto> lst = new List<QrHistoryDto>();

            var customerQrs = await _db.QrCodes
                .Where(x => x.CustomerId == id)
                .Include(x => x.Customer)
                .Include(x => x.Prize)
                .ThenInclude(x => x.Translations)
                .Include(x => x.ProductCampaign)
                .ToListAsync();

            foreach (var qr in customerQrs)
            {
                var qrHistory = new QrHistoryDto();

                qrHistory.ScannedDate = qr.WinDate;
                qrHistory.CustomerId = qr.CustomerId.Value;
                qrHistory.Value = qr.Value;
                qrHistory.IsPrizeReceived = qr.IsWinReceived;
                qrHistory.HasCustomerConfirmed = qr.HasCustomerConfirmed;
                qrHistory.Points = qr.ProductCampaign.Point;

                if (qr.PrizeId is not null)
                {
                    qrHistory.PrizeId = qr.PrizeId.Value;
                    qrHistory.Prize = new PrizeDto
                    {
                        Name = qr.Prize.Translations.FirstOrDefault().Name,
                        Link = qr.Prize.Link,
                        IsLeaderboard = qr.Prize.IsLeaderboard,
                        Id = qr.PrizeId.Value, 
                    };
                }

                lst.Add(qrHistory);
            }

            return lst.OrderByDescending(x => x.ScannedDate).ToList();
        }
    }
}
