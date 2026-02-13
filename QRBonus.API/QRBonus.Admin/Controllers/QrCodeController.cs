using Ardalis.Result;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Services.QrCodeService;
using QRBonus.DTO;
using QRBonus.DTO.QrCodeDtos;
using QRBonus.Shared.Enums;

namespace QRBonus.Admin.Controllers;

public class QrCodeController : ApiControllerBase
{
    public readonly IQrCodeAdminService _qrCodeService;

    public QrCodeController(IQrCodeAdminService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }

    [AllowAnonymous]
    [Authorize(Roles = $"{nameof(UserRole.Courier)}, {nameof(UserRole.Admin)}")]
    [HttpGet]
    public async Task<PagedResult<List<QrCodeDto>>> GetAll([FromQuery] QrCodeFilter filter)
    {
        return await _qrCodeService.GetAll(filter);
    }

    [HttpGet("win-export-excel")]
    public async Task<FileResult> ExportToExcel([FromQuery] QrCodeFilter filter)
    {
        var file =  await _qrCodeService.ExportWinPrizeToExcel(filter);

        return await ReturnExcel(file.Value.FileContent, file.Value.FileName);
    }

    [HttpGet("{id}")]
    public async Task<Result<QrCodeDto>> GetById(long id)
    {
        return await _qrCodeService.GetById(id);
    } 
    
    [HttpGet("history")]
    public async Task<Result<List<QrHistoryDto>>> CustomerHistory([FromQuery]long customerId)
    {
        return await _qrCodeService.CustomerHistory(customerId);
    }

    [HttpPost]
    public async Task<Result<BaseDto>> Add([FromBody] AddQrCodeDto dto)
    {
        return await _qrCodeService.Add(dto);
    }

    [AllowAnonymous]
    [Authorize(Roles = $"{nameof(UserRole.Courier)}, {nameof(UserRole.Admin)}")]
    [HttpPost("{id}/deliver")]
    public async Task<Result> WinDeliver(long id)
    {
        return await _qrCodeService.WinDeliver(id);
    }

    [HttpGet("export-csv")]
    public async Task<FileResult> ExportToCsv([FromQuery] QrCodeFilter filter)
    {
        var file = await _qrCodeService.ExportToCsv(filter);

        //List<string> aa = new List<string>();

        //for (int i = 0; i < 100; i++)
        //{
        //    aa.Add($"https://www.martinpamach.ge?qrCode=6229edcb3a474c4aa9de1948e9c300ac75795b3f128f4232b0af02647b50763f{Guid.NewGuid().ToString("N")}");
        //}

        return await ReturnCsv(file.Value.FileContent, file.Value.FileName);
    }
    
    private async Task<FileContentResult> ReturnCsv(byte[] csv, string fileName)
    {
        return await Task.FromResult(new FileContentResult(csv, "text/csv")
        {
            FileDownloadName = fileName

        });
    }

    private async Task<FileResult> ReturnExcel(byte[] excel, string name)
    {
        return await Task.FromResult(File(excel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            name));
    }
}