using QRBonus.DAL.Models;
using QRBonus.DTO.UserDtos;

using Riok.Mapperly.Abstractions;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class UserSessionMapper
    {
        public static partial UserSessionDto MapToUserSessionDto(this UserSession user);

    }
}
