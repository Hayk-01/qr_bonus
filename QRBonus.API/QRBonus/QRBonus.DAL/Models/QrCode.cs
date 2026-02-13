using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class QrCode : BaseWithRegionEntity
    {
        public string Value { get; set; }
        public long ProductCampaignId { get; set; }
        public long? PrizeId { get; set; }
        public long? CustomerId { get; set; }
        public DateTime? WinDate { get; set; }
        public bool IsWinReceived { get; set; }
        public bool HasCustomerConfirmed { get; set; }
        public bool IsExported { get; set; }
        public DateTime? PrizeDeliveryDate { get; set; }
        public DateTime? PrizeReceiveDate { get; set; }
        public long? UserId { get; set; }     //Araqich
        public DateTime ActivationDate { get; set; }
        public long? NotGivenPrizeId { get; set; }


        public ProductCampaign? ProductCampaign { get; set; }
        public Prize? Prize { get; set; }
        public Customer? Customer { get; set; }
        public User? User { get; set; }
    }
}
