using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.BannerDtos;
using QRBonus.DTO.PrizeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.BannerService
{
    public interface IBannerService
    {
        Task<Result<BannerDto>> GetById(long id);
        Task<PagedResult<List<BannerDto>>> GetAll(BannerFilter filter);

    }
}
