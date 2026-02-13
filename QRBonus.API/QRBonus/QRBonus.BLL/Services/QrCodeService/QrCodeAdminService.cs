using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.QrCodeDtos;
using Microsoft.EntityFrameworkCore;
using QRBonus.DAL.Models;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using Microsoft.Extensions.Options;
using QRBonus.BLL.Models;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL;
using EFCore.BulkExtensions;
using QRBonus.BLL.Services.UserService;
using System.Data;
using Twilio.TwiML.Fax;

namespace QRBonus.BLL.Services.QrCodeService
{
    public class QrCodeAdminService : QrCodeBaseService, IQrCodeAdminService
    {
        private readonly MartinSettings _options;
        private readonly IErrorService _errorService;
        private readonly IUserSessionService _userSessionService;
        private readonly LanguageConfiguration _languageConfiguration;

        public QrCodeAdminService(AppDbContext db, IOptions<MartinSettings> options, IErrorService errorService, IUserSessionService userSessionService, LanguageConfiguration languageConfiguration) :
            base(db)
        {
            _options = options.Value;
            _errorService = errorService;
            _userSessionService = userSessionService;
            _languageConfiguration = languageConfiguration;
        }

        public async Task<Result> AddOld(AddQrCodeDto dto)
        {
            foreach (var qrCodeCount in dto.QrCodeCounts)
            {
                for (int i = 0; i < qrCodeCount.Count; i++)
                {
                    var qrCode = new QrCode
                    {
                        Value = "http://martinpamach.ge/?qrcode=" + Guid.NewGuid().ToString("N") +
                                Guid.NewGuid().ToString("N"),
                        ProductCampaignId = dto.ProductCampaignId,
                        PrizeId = qrCodeCount.PrizeId,
                    };

                    _db.Add(qrCode);
                }
            }

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Add(AddQrCodeDto dto)
        {
            var random = new Random();

            var dict = new Dictionary<long, int>();

            foreach (var d in dto.QrCodeCounts)
            {
                if (d.PrizeId.HasValue)
                {
                    dict[d.PrizeId.Value] = d.Count;
                }
            }

            var totalCount = dto.QrCodeCounts.Sum(x => x.Count);

            var numbers = Enumerable.Range(0, totalCount)
                .OrderBy(x => random.Next())
                .Take(dict.Select(x => x.Value).Sum(x => x))
                .ToList();

            var lst = new List<(int, long)>();

            foreach (var d in dict)
            {
                var c = numbers.Take(d.Value).ToList();
                
                foreach (var number in c)
                {
                    lst.Add((number, d.Key));
                }

                numbers.RemoveRange(0, c.Count);
            }

            var entities = new List<QrCode>();

            var regionId = (await _db.ProductCampaigns.Include(x => x.Campaign).FirstAsync(x => x.Id == dto.ProductCampaignId)).Campaign!.RegionId;

            for (int i = 0; i < totalCount; i++)
            {
                await Task.Delay(1);
                
                var qrCode = new QrCode
                {
                    Value = _options.QrUrl + UniqueIdGenerator.Generate16CharId(),
                    ProductCampaignId = dto.ProductCampaignId,
                    CreatedDate = DateTime.UtcNow,
                    ModifyDate = DateTime.UtcNow,
                    ActivationDate = dto.ActivationDate,
                    RegionId = regionId
                };

                entities.Add(qrCode);
            }

            foreach (var (number, prizeId) in lst)
            {
                entities[number].PrizeId = prizeId;
            }

            await _db.BulkInsertAsync(entities);

            return Result.Success();
        }

        public async Task<Result> AddOld2(AddQrCodeDto dto)
        {
            var dict = new Dictionary<long, long>();

            foreach (var d in dto.QrCodeCounts)
            {
                if (d.PrizeId.HasValue)
                {
                    dict[d.PrizeId.Value] = d.Count;
                }
            }

            var totalCount = dto.QrCodeCounts.Sum(x => x.Count);
            var entities = new List<QrCode>();

            for (int i = 0; i < totalCount; i++)
            {
                var randomPrizeId = GetRandomPrizeId(totalCount, i, dict);

                var qrCode = new QrCode
                {
                    Value = _options.QrUrl + Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
                    ProductCampaignId = dto.ProductCampaignId,
                    PrizeId = randomPrizeId,
                };

                entities.Add(qrCode);

                if (randomPrizeId.HasValue)
                {
                    dict[randomPrizeId.Value]--;
                }
            }

            await _db.BulkInsertAsync(entities);

            return Result.Success();
        }

        private long? GetRandomPrizeId(long totalCount, int currentIndex, Dictionary<long, long> prizeIds)
        {
            Random random = new Random();

            long[] ints = new long[totalCount - currentIndex];

            List<long> prizes = new List<long>();

            foreach (var prizeId in prizeIds)
            {
                for (int i = 0; i < prizeId.Value; i++)
                {
                    prizes.Add(prizeId.Key);
                }
            }

            for (int i = 0; i < prizes.Count; i++)
            {
                ints[i] = prizes[i];
            }

            var selectedPrizeId = ints[random.Next(ints.Length)];

            return selectedPrizeId == 0 ? null : selectedPrizeId;
        }

        public async Task<Result> Delete(long id)
        {
            var qrCode = await _db.QrCodes.FirstOrDefaultAsync(x => x.Id == id);

            if (qrCode == null || qrCode.IsDeleted)
            {
                return Result.NotFound();
            }

            qrCode.IsDeleted = true;
            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<PagedResult<List<QrCodeDto>>> GetAll(QrCodeFilter filter)
        {
            var query = _db.QrCodes
                .Include(x => x.Customer)
                .Include(x => x.Prize)
                .Include(x => x.User)
                .Include(x => x.Region)
                .AsQueryable();

            var entities = await filter.FilterObjects(query)
                .ToListAsync();

            List<QrCodeDto> result = entities.MapToQrCodeDtos();

            return new PagedResult<List<QrCodeDto>>(await filter.GetPagedInfoAsync(query), result);
        }

        public async Task<Result<QrCodeDto>> GetById(long id)
        {
            var qrCode = await _db.QrCodes.FirstOrDefaultAsync(x => x.Id == id);

            if (qrCode == null)
            {
                return Result.NotFound();
            }

            var dto = qrCode.MapToQrCodeDto();

            return dto;
        }

        public async Task<Result<FileExport>> ExportToCsv(QrCodeFilter filter)
        {
            var query = _db.QrCodes.AsQueryable();

            filter.Skip = 1;
            filter.Take = null;

            var qrCodes = await filter.FilterObjects(query).ToListAsync();

            byte[] exportData = await CsvExportHelper.ExportAsync(qrCodes.Select(x => x.Value).ToList());

            var result = new FileExport
            {
                FileContent = exportData,
                FileName = $"{filter.ProductCampaignId}_{DateTime.Now.ToString("yyyy_mm_dd")}"
            };

            foreach (var qr in qrCodes)
            {
                qr.IsExported = true;
            }

            await _db.SaveChangesAsync();

            return result;
        }

        public async Task<Result> WinDeliver(long id)
        {
            var qrCode = await _db.QrCodes.FirstOrDefaultAsync(x => x.Id == id);

            if (qrCode == null || qrCode.CustomerId == null)
            {
                return Result.NotFound();
            }

            if (qrCode.IsWinReceived)
            {
                return Result.Error("Prize is already delivered");
            }

            qrCode.IsWinReceived = true;
            qrCode.PrizeDeliveryDate = DateTime.UtcNow;
            qrCode.UserId = _userSessionService.CurrentUser.Id;

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<FileExport>> ExportWinPrizeToExcel(QrCodeFilter filter)
        {
            filter.Skip = 1;
            filter.Take = null;

            filter.HasPrize = true;
            filter.IsExported = true;

            List<QrCodeDto> report = await GetAll(filter);

            var dataTable = new DataTable();

            var translations = new Dictionary<int, List<string>>
            {
                [1] = ["ID", "Action", "Status", "Courier first name", "Courier last name", "Date of delivery",
                    "Date of receipt", "Prize Id", "Customer Id", "Customer First Name", "Customer Last Name",
                    "Customer Phone Number", "Create Date", "Is Exported", "Region"], //Eng

                [2] = ["ID", "Action", "Status", "Courier first name", "Courier last name", "Date of delivery",
                    "Date of receipt", "Prize Id", "Customer Id", "Customer First Name", "Customer Last Name",
                    "Customer Phone Number", "Create Date", "Is Exported", "Region"],  //Geo
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

                row[0] = r.Id;                        //ID
                row[1] = r.Customer == null
                    ? "---"
                    : r.IsWinReceived
                        ? "Prize is received"
                        : "Prize is not received";     //Action
                row[2] = r.Customer == null
                        ? "---"
                        : !r.IsWinReceived
                        ? "Prize has not been given"
                        : !r.HasCustomerConfirmed
                            ? "Prize has been given by courier"
                            : "Confirmeed by customer";    //"Status"
                row[3] = r.User?.FirstName ?? string.Empty;  //"Courier first name"
                row[4] = r.User?.LastName ?? string.Empty; //"Courier last name" 
                row[5] = r.PrizeDeliveryDate == null ? "" : r.PrizeDeliveryDate!.Value.AddHours(4);    //"Date of delivery"
                row[6] = r.PrizeReceiveDate == null ? "" : r.PrizeReceiveDate!.Value.AddHours(4);      // "Date of receipt"
                row[7] = r.Prize?.Id;            // "Prize Id"
                row[8] = r.Customer?.Id;         //"Customer Id"
                row[9] = r.Customer?.FirstName ?? string.Empty; //, "Customer First Name"
                row[10] = r.Customer?.LastName ?? string.Empty; //"Customer Last Name"                             
                row[11] = r.Customer?.PhoneNumber ?? string.Empty; // "Customer Phone Number"
                row[12] = r.CreatedDate.AddHours(4);     // "Create Date"
                row[13] = r.IsExported ? "Yes" : "No";      // "Is Exported"
                row[14] = r.RegionId == 1 ? "Georgia" : "Armenia";         // "Region"

                dataTable.Rows.Add(row);
            }

            return new FileExport
            {
                FileContent = ExcelExportHelper.PrepareExcel(dataTable),
                FileName = $"Qr_Codes_With_Prize_{DateTime.Now.Date.ToString("dd.MM.yy")}.xlsx"
            };
        }
    }  
}