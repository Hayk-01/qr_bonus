using Ardalis.Result;
using QRBonus.BLL.Filters;

using QRBonus.DTO.PrizeDtos;

namespace QRBonus.BLL.Services.PrizeService
{
    public interface IPrizeService
    {
        Task<Result<PrizeDto>> GetById(long id);
        Task<PagedResult<List<PrizeDto>>> GetAll(PrizeFilter filter);

    }
}
