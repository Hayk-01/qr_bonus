using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.CustomerDtos
{
    public class CustomerSessionDto
    {
        public long CustomerId { get; set; }
        public string Token { get; set; } = default!;
        public bool IsExpired { get; set; }
        public CustomerDto? Customer { get; set; }
    }
}
