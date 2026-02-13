using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class CustomerSession : BaseEntity
    {
        public long CustomerId { get; set; }
        public string Token { get; set; } = default!;
        public bool IsExpired { get; set; }
        public Customer? Customer { get; set; }

    }
}
