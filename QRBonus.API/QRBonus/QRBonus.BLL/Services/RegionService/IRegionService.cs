using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO.RegionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.RegionService
{
    public interface IRegionService
    {
        Task<PagedResult<List<RegionDto>>> GetAll(RegionFilter filter);
        Task<Result<RegionDto>> GetById(long id);

    }
}
