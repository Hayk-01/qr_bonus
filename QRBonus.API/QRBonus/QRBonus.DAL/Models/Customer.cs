using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.Shared.Enums;

namespace QRBonus.DAL.Models
{
    public class Customer : BaseWithRegionEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public string? VerificationCode { get; set; }
        public string? Token { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}