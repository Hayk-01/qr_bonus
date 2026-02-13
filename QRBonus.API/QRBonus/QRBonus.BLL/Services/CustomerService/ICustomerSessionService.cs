using Ardalis.Result;
using QRBonus.DAL.Models;
using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.CustomerService
{
    public interface ICustomerSessionService
    {
        Customer CurrentCustomer { get; }
        Task<Result> Login(CustomerLoginDto dto);
        Task<Result> SendVerificationCode(string phoneNumber);
        Task<Result> LogOut();
        Task<Result<CustomerSessionDto>> VerifyCustomer(CustomerVerificationDto dto);
        Task<CustomerDto?> GetByToken(string token);
    }
}
