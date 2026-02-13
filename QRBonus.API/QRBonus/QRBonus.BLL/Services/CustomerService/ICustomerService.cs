using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.ProductDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.DTO.CustomerDtos;

namespace QRBonus.BLL.Services.CustomerService
{
    public interface ICustomerService
    {
        Task<PagedResult<List<CustomerDto>>> GetAll(CustomerFilter filter);
        Task<Result<CustomerDto>> GetById(long id);
        Task<Result> Update(long id, CustomerDto dto);
        Task<Result<List<CustomerPrizePointDto>>> GetPrizesAndPoints(long id);
        Task<Result<CustomerRankDto>> GetRank(long id);
        Task<Result> Delete(long id);
        Task<Result> UpdateToken(long userId, UpdateCustomerFireBaseTokenDto fireBaseToken,
            long currentCustomerRegionId);
    }
}
