using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.PrizeDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.DTO.BannerDtos;

namespace QRBonus.BLL.Services.BannerService
{
    public interface IBannerAdminService
    {
        Task<Result<AddOrUpdateBannerDto>> GetByIdAdmin(long id);
        Task<PagedResult<List<BannerDto>>> GetAll(BannerFilter filter);
        Task<Result<BaseDto>> Add(AddOrUpdateBannerDto dto);
        Task<Result> Update(long id, AddOrUpdateBannerDto dto);
        Task<Result> Delete(long id);
    }
}
