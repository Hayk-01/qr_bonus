using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.BLL.Services.QrCodeService;
using QRBonus.DTO.QrCodeDtos;

namespace QRBonus.API.Controllers;

public class QrCodeController : ApiControllerBase
{
    private readonly IQrCodeService _qrCodeService; 
    private readonly ICustomerSessionService _customerSessionService;

    public QrCodeController(IQrCodeService qrCodeService, ICustomerSessionService customerSessionService)
    {
        _qrCodeService = qrCodeService;
        _customerSessionService = customerSessionService;
    }


    [HttpPost("scan")]
    public async Task<Result<ScannedQrDto>> Scan(string qr)
    {
        return await _qrCodeService.Scan(qr);
    }

    [HttpGet("history")]
    public async Task<Result<List<QrHistoryDto>>> CustomerHistory()
    {
        return await _qrCodeService.CustomerHistory(_customerSessionService.CurrentCustomer.Id);
    }

    [HttpGet("history-item")]
    public async Task<Result<QrHistoryDto>> CustomerHistory(string qr)
    {
        return await _qrCodeService.CustomerHistory(_customerSessionService.CurrentCustomer.Id, qr);
    }

    [HttpPost("win-receive")]
    public async Task<Result> WinReceive(string qr)
    {
        return await _qrCodeService.WinReceiveConfirmation(qr);
    }
}
