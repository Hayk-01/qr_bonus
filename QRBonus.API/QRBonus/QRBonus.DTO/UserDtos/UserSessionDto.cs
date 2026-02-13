using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.UserDtos
{
    public class UserSessionDto
    {
        public long UserId { get; set; }
        public string Token { get; set; } = default!;
        public bool IsExpired { get; set; }
        public UserDto User { get; set; }
    }
}
