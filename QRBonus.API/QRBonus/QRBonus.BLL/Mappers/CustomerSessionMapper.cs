using QRBonus.DAL.Models;
using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO.UserDtos;
using Riok.Mapperly.Abstractions;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class CustomerSessionMapper
    {
        public static partial CustomerSessionDto MapToCustomerSessionDto(this CustomerSession user);

    }
}
