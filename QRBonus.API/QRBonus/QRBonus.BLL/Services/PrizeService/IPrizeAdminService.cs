using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO;

using QRBonus.DTO.PrizeDtos;

namespace QRBonus.BLL.Services.PrizeService
{
    public interface IPrizeAdminService
    {
        Task<Result<AddOrUpdatePrizeDto>> GetByIdAdmin(long id);
        Task<PagedResult<List<PrizeDto>>> GetAll(PrizeFilter filter);
        Task<Result<BaseDto>> Add(AddOrUpdatePrizeDto dto);
        Task<Result> Update(long id, AddOrUpdatePrizeDto dto);
        Task<Result> Delete(long id);

    } 
}
