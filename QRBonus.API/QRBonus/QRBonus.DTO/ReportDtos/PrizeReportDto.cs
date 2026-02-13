using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.ReportDtos
{
    public class PrizeReportDto
    {
        public long? PrizeId { get; set; }
        public string? PrizeName { get; set; }
        public long QrTotalQuantity { get; set; }
        public long WinTotalQuantity { get; set; }
        public long ClaimedPrizeQuantity { get; set; }
        public long UnclaimedPrizeQuantity { get; set; }

    }
}
