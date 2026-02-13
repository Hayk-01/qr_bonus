using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.QrCodeDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.DAL;
using Microsoft.AspNetCore.Mvc;
using QRBonus.DAL.Models;

namespace QRBonus.BLL.Services.QrCodeService
{
    public interface IQrCodeAdminService
    {
        Task<Result<QrCodeDto>> GetById(long id);
        Task<PagedResult<List<QrCodeDto>>> GetAll(QrCodeFilter filter);
        Task<Result> Add(AddQrCodeDto dto);
        Task<Result> Delete(long id);
        Task<Result<List<QrHistoryDto>>> CustomerHistory(long id);
        Task<Result> WinDeliver(long id);
        Task<Result<FileExport>> ExportToCsv(QrCodeFilter filter);
        Task<Result<FileExport>> ExportWinPrizeToExcel(QrCodeFilter filter);
    }
}
