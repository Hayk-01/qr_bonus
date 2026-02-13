using QRBonus.DAL.Models;
using QRBonus.DTO.QrCodeDtos;
using Riok.Mapperly.Abstractions;

namespace QRBonus.BLL.Mappers;

[Mapper(UseReferenceHandling = true)]
public static partial class QrCodeMapper
{
    public static partial QrCodeDto MapToQrCodeDto(this QrCode qrCode);

    public static partial List<QrCodeDto> MapToQrCodeDtos(this List<QrCode> qrCode);

}