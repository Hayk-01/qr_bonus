using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.Shared.Enums;

namespace QRBonus.DTO.CustomerDtos
{
    public class CustomerDto : BaseWithRegionDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public long? ShoeSize { get; set; }
    }

    public class UpdateCustomerFireBaseTokenDto
    {
        public string Token { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
