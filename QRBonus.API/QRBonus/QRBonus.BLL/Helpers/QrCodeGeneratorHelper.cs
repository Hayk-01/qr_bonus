using QRBonus.DAL.Models;
using QRBonus.DTO.QrCodeDtos;
using QRCoder;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using ZXing;

namespace QRBonus.BLL.Helpers
{
    public static class QrCodeGeneratorHelper
    {
        public static byte[] GenerateQrCodeByte(string text)
        {
            byte[] qrCode = new byte[0];

            if (!string.IsNullOrEmpty(text))
            {
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData data = qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode bitMap = new PngByteQRCode(data);

                qrCode = bitMap.GetGraphic(20, false);
            }

            return qrCode;
        }
    }
}
