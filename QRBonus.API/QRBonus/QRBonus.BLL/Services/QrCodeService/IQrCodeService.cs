using Ardalis.Result;
using QRBonus.DTO.QrCodeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.QrCodeService
{
    public interface IQrCodeService
    {
        public Task<Result<ScannedQrDto>> Scan(string qrCode);
        Task<Result<List<QrHistoryDto>>> CustomerHistory(long id);
        Task<Result<QrHistoryDto>> CustomerHistory(long customerId, string qr);
        Task<Result> WinReceiveConfirmation(string qr);
    }
}
