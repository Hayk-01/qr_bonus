using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.QrCodeDtos
{
    public class QrHistoryDto
    {
        public string Value { get; set; }
        public DateTime? ScannedDate { get; set; }
        public long CustomerId { get; set; }
        public long? PrizeId { get; set; }
        public long? Points { get; set; }
        public bool IsPrizeReceived { get; set; }
        public bool HasCustomerConfirmed { get; set; }
        public DateTime? PrizeDeliveryDate { get; set; }
        public DateTime? PrizeReceiveDate { get; set; }
        public long? UserId { get; set; }
        public DateTime ActivationDate { get; set; }



        public CustomerDto? Customer { get; set; }
        public PrizeDto? Prize { get; set; }
        public UserDto? User { get; set; }
    }
}    