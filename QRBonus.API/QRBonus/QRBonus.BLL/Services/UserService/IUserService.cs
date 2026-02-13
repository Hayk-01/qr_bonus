using Ardalis.Result;
using QRBonus.DTO.UserDtos;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRBonus.BLL.Filters;

namespace QRBonus.BLL.Services.UserService
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetById(long id);
        Task<PagedResult<List<UserDto>>> GetAll(UserFilter filter);
        Task<Result<BaseDto>> Add(AddUserDto addUserDto);
        Task<Result> Update(long id, UpdateUserDto updateUserDto);
        Task<Result> Delete(long id);
        Task<Result<UserDto>> GetMe();

    }
}
