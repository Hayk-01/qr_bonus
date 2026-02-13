using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.CustomerDtos
{
    public class CustomerLoginDto
    {
        public string PhoneNumber { get; set; } = default!;
        public long RegionId { get; set; }
    }
}
