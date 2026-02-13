using Ardalis.Result;
using QRBonus.BLL.Filters;
using QRBonus.DTO;
using QRBonus.DTO.RegionDtos;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.RegionService
{
    public interface IRegionAdminService
    {
        Task<Result<AddOrUpdateRegionDto>> GetByIdAdmin(long id);
        Task<PagedResult<List<RegionDto>>> GetAll(RegionFilter filter);
        Task<Result<BaseDto>> Add(AddOrUpdateRegionDto dto);
        Task<Result> Update(long id, AddOrUpdateRegionDto dto);
        Task<Result> Delete(long id);
    } 
}