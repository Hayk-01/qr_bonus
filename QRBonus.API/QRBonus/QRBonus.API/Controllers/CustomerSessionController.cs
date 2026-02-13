using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.BLL.Services.UserService;
using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO.UserDtos;

namespace QRBonus.API.Controllers
{
    public class CustomerSessionController : ApiControllerBase
    {
        private readonly ICustomerSessionService _customerSessionService;

        public CustomerSessionController(ICustomerSessionService customerSessionService)
        {
            _customerSessionService = customerSessionService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Result> Login(CustomerLoginDto dto)
        {
            return await _customerSessionService.Login(dto);
        }

        [AllowAnonymous]
        [HttpPut("verify")]
        public async Task<Result<CustomerSessionDto>> VerifyCustomer([FromBody] CustomerVerificationDto dto)
        {
            return await _customerSessionService.VerifyCustomer(dto);
        }

        [HttpPost("logout")]
        public async Task<Result> LogOut()
        {
            return await _customerSessionService.LogOut();
        }
    }
}
