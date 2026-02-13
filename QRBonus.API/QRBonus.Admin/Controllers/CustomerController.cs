using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.DTO.CustomerDtos;

namespace QRBonus.Admin.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        private readonly ICustomerAdminService _customerAdminService;

        public CustomerController(ICustomerAdminService customerAdminService)
        {
            _customerAdminService = customerAdminService;
        }

        [HttpGet]
        public async Task<PagedResult<List<CustomerDto>>> GetAll([FromQuery] CustomerFilter filter)
        {
            return await _customerAdminService.GetAll(filter);
        }

        [HttpGet("{id}")]
        public async Task<Result<CustomerDto>> GetById(long id)
        {
            return await _customerAdminService.GetById(id);
        }

        [HttpGet("{id}/points")]
        public async Task<Result<List<CustomerPrizePointDto>>> GetPoints(long id)
        {
            return await _customerAdminService.GetPrizesAndPoints(id);
        }

        [HttpGet("{id}/rank")]
        public async Task<Result<CustomerRankDto>> GetRank(long id)
        {
            return await _customerAdminService.GetRank(id);
        }

        [HttpDelete("{id}")]
        public async Task<Result> Delete(long id)
        {
            return await _customerAdminService.Delete(id);
        }


    }
}
