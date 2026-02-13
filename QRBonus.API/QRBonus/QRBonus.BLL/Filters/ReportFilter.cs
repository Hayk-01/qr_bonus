using InnLine.BLL.Filters;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Filters
{
    public class ReportFilter : BaseFilter<QrCode>
    {
        public long? PrizeId { get; set; }
        public List<long>? PrizeIds { get; set; }
        public DateTime? FromCreatedDate { get; set; }
        public DateTime? ToCreatedDate { get; set; }
        public DateTime? FromWinDate { get; set; }
        public DateTime? ToWinDate { get; set; }
        public List<long>? RegionIds { get; set; }

        public override IQueryable<QrCode> CreateQuery(IQueryable<QrCode> query)
        {
            if (PrizeId.HasValue)
            {
                query = query.Where(x => x.PrizeId == PrizeId);
            }

            if (FromCreatedDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= FromCreatedDate);
            }

            if (ToCreatedDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= ToCreatedDate);
            }

            if (FromWinDate.HasValue)
            {
                query = query.Where(x => x.WinDate >= FromWinDate);
            }

            if (ToWinDate.HasValue)
            {
                query = query.Where(x => x.WinDate <= ToWinDate);
            }

            if (PrizeIds != null && PrizeIds.Any())
            {
                query = query.Where(x => PrizeIds.Contains(x.PrizeId.Value));
            }

            if (RegionIds != null && RegionIds.Any())
            {
                query = query.Where(x => RegionIds.Contains(x.ProductCampaign.Campaign.RegionId));
            }
            return query.OrderByDescending(x => x.Id);
        }
    }
}
