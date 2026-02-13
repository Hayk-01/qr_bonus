using QRBonus.DAL.Models;
using QRBonus.DTO.UserDtos;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class UserMapper
    {
        public static partial UserDto MapToUserDto(this User user);

        public static partial List<UserDto> MapToUserDtos(this List<User> users);
    }
}
