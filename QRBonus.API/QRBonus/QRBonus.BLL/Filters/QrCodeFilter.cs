using InnLine.BLL.Filters;
using QRBonus.DAL.Models;

namespace QRBonus.BLL.Filters
{
    public class QrCodeFilter : BaseFilter<QrCode>
    {
        public long? ProductCampaignId { get; set; }
        public long? PrizeId { get; set; }
        public bool? HasPrize { get; set; }
        public long? CustomerId { get; set; }
        public DateTime? WinDate { get; set; }
        public bool? IsWinReceived { get; set; }
        public bool? HasCustomerConfirmed { get; set; }
        public bool? IsExported { get; set; }
        public string? Value { get; set; }
        public List<long>? RegionIds { get; set; }
        public DateTime? PrizeDeliveryDate { get; set; }
        public DateTime? PrizeReceiveDate { get; set; }
        public DateTime? PrizeReceiveDateFrom { get; set; }
        public DateTime? PrizeReceiveDateTo { get; set; }
        public List<long>? UserIds { get; set; }



        public override IQueryable<QrCode> CreateQuery(IQueryable<QrCode> query)
        {         
            if (ProductCampaignId.HasValue)
            {
                query = query.Where(x => x.ProductCampaignId == ProductCampaignId.Value);
            }

            if (PrizeId.HasValue)
            {
                query = query.Where(x => x.PrizeId == PrizeId.Value);
            }

            if (HasPrize.HasValue)
            {
                query = query.Where(x => x.PrizeId.HasValue == HasPrize.Value);
            }

            if (CustomerId.HasValue)
            {
                query = query.Where(x => x.CustomerId == CustomerId.Value);
            }

            if(WinDate.HasValue)
            {
                query = query.Where(x => x.WinDate.Value.Date == WinDate.Value.Date);
            }
             
            if(PrizeReceiveDate.HasValue)
            {
                query = query.Where(x => x.PrizeReceiveDate.Value.Date == PrizeReceiveDate.Value.Date);
            } 
            
            if(PrizeDeliveryDate.HasValue)
            {
                query = query.Where(x => x.PrizeDeliveryDate.Value.Date == PrizeDeliveryDate.Value.Date);
            }
            
            if(PrizeReceiveDateFrom.HasValue)
            {
                query = query.Where(x => x.PrizeReceiveDate >= PrizeReceiveDateFrom);
            }

            if(PrizeReceiveDateTo.HasValue)
            {
                query = query.Where(x => x.PrizeReceiveDate <= PrizeReceiveDateTo);
            }
            
            if(IsWinReceived.HasValue)
            {
                query = query.Where(x => x.IsWinReceived == IsWinReceived.Value);
            }
            
            if(HasCustomerConfirmed.HasValue)
            {
                query = query.Where(x => x.HasCustomerConfirmed == HasCustomerConfirmed.Value);
            }

            if (IsExported.HasValue)
            {
                query = query.Where(x => x.IsExported == IsExported.Value);
            }

            if (!string.IsNullOrWhiteSpace(Value))
            {
                query = query.Where(x => x.Value == Value);
            }

            if (RegionIds != null && RegionIds.Any())
            {
                query = query.Where(x => RegionIds.Contains(x.RegionId));
            }
            
            if (UserIds != null && UserIds.Any())
            {
                query = query.Where(x => UserIds.Contains(x.User.Id));
            }

            return query.OrderByDescending(x => x.Id);
        }
    }
}
