using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.PrizeService;
using QRBonus.BLL.Services.ReportService;
using QRBonus.DTO.ReportDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QRBonus.Admin.Controllers
{
    public class ReportController : ApiControllerBase
    {
        private readonly IReportAdminService _reportAdminService;
        public ReportController(IReportAdminService reportAdminService)
        {
            _reportAdminService = reportAdminService;
        }

        [HttpGet("prize")]
        public async Task<Result<List<PrizeReportDto>>> PrizeReport([FromQuery] ReportFilter filter)
        {
            return await _reportAdminService.PrizeReport(filter);
        }

        [HttpGet("prize/export")]
        public async Task<FileResult> PrizeReportExport([FromQuery] ReportFilter filter)
        {
            var exp = await _reportAdminService.PrizeReportExport(filter);

            return await ReturnExcel(exp.FileContent, exp.FileName);

        }

        private async Task<FileResult> ReturnExcel(byte[] excel, string name)
        {
            return await Task.FromResult(File(excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                name));
        }
    }
}
