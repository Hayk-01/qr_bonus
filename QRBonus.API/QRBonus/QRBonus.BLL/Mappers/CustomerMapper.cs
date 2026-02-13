using QRBonus.DAL.Models;
using QRBonus.DTO.CustomerDtos;
using Riok.Mapperly.Abstractions;
using System.Reflection;

namespace QRBonus.BLL.Mappers
{
    [Mapper]
    public static partial class CustomerMapper
    {
        public static CustomerDto MapToCustomerDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                CreatedDate = customer.CreatedDate,
                ModifyDate = customer.ModifyDate,
                Region = customer.Region?.MapToRegionDto(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                RegionId = customer.RegionId
            };
        }

        public static partial List<CustomerDto> MapToCustomerDtos(this List<Customer> customers);
    }
}