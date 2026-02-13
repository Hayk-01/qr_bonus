using Ardalis.Result;
using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.BLL.Filters;

namespace QRBonus.BLL.Services.CustomerService
{
    public interface ICustomerAdminService
    {
        Task<Result> Delete(long id);
        Task<PagedResult<List<CustomerDto>>> GetAll(CustomerFilter filter);
        Task<Result<CustomerDto>> GetById(long id);
        Task<Result<List<CustomerPrizePointDto>>> GetPrizesAndPoints(long id);
        Task<Result<CustomerRankDto>> GetRank(long id);

        //Task<Result> UpdateStatus(long id);
    }
}
