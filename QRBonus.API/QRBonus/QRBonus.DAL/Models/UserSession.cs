using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DAL.Models
{
    public class UserSession : BaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; } = default!;
        public bool IsExpired { get; set; }

        public User? User { get; set; }
    }
}
