using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO.ReportDtos;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.ReportService
{
    public class ReportAdminService : IReportAdminService
    {
        private readonly AppDbContext _db;
        private readonly LanguageConfiguration _languageConfiguration;


        public ReportAdminService(AppDbContext db, LanguageConfiguration languageConfiguration)
        {
            _db = db;
            _languageConfiguration = languageConfiguration;
        }
        public async Task<Result<List<PrizeReportDto>>> PrizeReport(ReportFilter filter)
        {
            filter.Skip = 1;
            filter.Take = null;

            var query = _db.QrCodes
                .Include(x => x.Customer)
                .Include(x => x.ProductCampaign)
                .ThenInclude(x => x.Campaign)
                .Include(x => x.Prize)
                .ThenInclude(x => x.Translations)
                .Where(x => x.PrizeId != null)
                .AsQueryable()
                .AsSingleQuery();

            var qrCodes = await filter.FilterObjects(query)
                .GroupBy(x => x.PrizeId)
                .Select(x => new PrizeReportDto
                {
                    PrizeId = x.FirstOrDefault().PrizeId,
                    PrizeName = x.FirstOrDefault().Prize.Translations.FirstOrDefault().Name,
                    QrTotalQuantity = x.Count(),
                    WinTotalQuantity = x.Count(x => x.CustomerId != null),
                    ClaimedPrizeQuantity = x.Count(x => x.IsWinReceived),
                    UnclaimedPrizeQuantity = x.Count(x => !x.IsWinReceived),
                })
                .ToListAsync();

            return qrCodes;
        }

        public async Task<FileExport> PrizeReportExport(ReportFilter filter)
        {
            List<PrizeReportDto> report = await PrizeReport(filter);

            var dataTable = new DataTable();

            var translations = new Dictionary<int, List<string>>
            {
                [1] = ["Prize Id", "Prize Name", "Total Created", "Total Won", "Total Claimed", "Total Unclaimed"], //Eng

                [2] = ["Prize Id", "Prize Name", "Total Created", "Total Won", "Total Claimed", "Total Unclaimed"],  //Geo
            };

            if (translations.TryGetValue(_languageConfiguration.LanguageId, out var columnTranslations))
            {
                foreach (var column in columnTranslations)
                {
                    dataTable.Columns.Add(column);
                }
            }
            else
            {
                throw new Exception("Unsupported language ID");
            }

            foreach (var r in report)
            {
                var row = dataTable.NewRow();

                row[0] = r.PrizeId;
                row[1] = r.PrizeName;
                row[2] = r.QrTotalQuantity;
                row[3] = r.WinTotalQuantity;
                row[4] = r.ClaimedPrizeQuantity;
                row[5] = r.UnclaimedPrizeQuantity;

                dataTable.Rows.Add(row);
            }

            return new FileExport
            {
                FileContent = ExcelExportHelper.PrepareExcel(dataTable),
                FileName = $"Prize_Report_{DateTime.Now.Date.ToString("dd.MM.yy")}.xlsx"
            };
        }
    }
}