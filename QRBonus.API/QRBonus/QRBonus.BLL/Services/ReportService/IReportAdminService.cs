using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.ReportService
{
    public interface IReportAdminService
    {
        Task<Result<List<PrizeReportDto>>> PrizeReport (ReportFilter filter);
        Task<FileExport> PrizeReportExport(ReportFilter filter);
    }
}
