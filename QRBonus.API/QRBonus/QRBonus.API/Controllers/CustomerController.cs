using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO;
using QRBonus.DTO.CustomerDtos;
using Microsoft.AspNetCore.Authorization;

namespace QRBonus.API.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerSessionService _customerSessionService;

        public CustomerController(ICustomerService customerService, ICustomerSessionService customerSessionService)
        {
            _customerService = customerService;
            _customerSessionService = customerSessionService;
        }

        [HttpGet]
        public async Task<PagedResult<List<CustomerDto>>> GetAll([FromQuery] CustomerFilter filter)
        {
            return await _customerService.GetAll(filter);
        }

        [HttpGet("{id}")]
        public async Task<Result<CustomerDto>> GetById(long id)
        {
            return await _customerService.GetById(id);
        }
        
        [HttpPut("{id}")]
        public async Task<Result> Update(long id, [FromBody] CustomerDto dto)
        {
            return await _customerService.Update(id, dto);
        }

        [HttpPost("update-token")]
        public async Task<Result> UpdateToken([FromBody] UpdateCustomerFireBaseTokenDto fireBaseToken)
        {
            var userId = _customerSessionService.CurrentCustomer.Id;
            
            return await _customerService.UpdateToken(userId, fireBaseToken, _customerSessionService.CurrentCustomer.RegionId);
        }

        [HttpGet("me/points")]
        public async Task<Result<List<CustomerPrizePointDto>>> GetMyPoints()
        {
            return await _customerService.GetPrizesAndPoints(_customerSessionService.CurrentCustomer.Id);
        }

        [HttpGet("me/rank")]
        public async Task<Result<CustomerRankDto>> GetMyRank()
        {
            return await _customerService.GetRank(_customerSessionService.CurrentCustomer.Id);
        }

        [HttpDelete("me")]
        public async Task<Result> Delete()
        {
            return Result.Success();
            
            return await _customerService.Delete(_customerSessionService.CurrentCustomer.Id);
        }

    }
}
