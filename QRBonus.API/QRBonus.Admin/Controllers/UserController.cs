using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.UserService;
using QRBonus.DTO;
using QRBonus.DTO.UserDtos;

namespace QRBonus.Admin.Controllers;

public class UserController : ApiControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<Result<BaseDto>> Create(AddUserDto userDto)
    {
        return await _userService.Add(userDto);
    }

    [HttpGet("{id}")]
    public async Task<Result<UserDto>> GetById(long id)
    {
        return await _userService.GetById(id);
    }

    [HttpGet]
    public async Task<PagedResult<List<UserDto>>> GetAll([FromQuery] UserFilter filter)
    {
        return await _userService.GetAll(filter);
    }

    [HttpPut("{id}")]
    public async Task<Result<UserDto>> Update(long id, UpdateUserDto userDto)
    {
        return await _userService.Update(id, userDto);
    }

    [HttpDelete("{id}")]
    public async Task<Result> Delete(long id)
    {
        return await _userService.Delete(id);
    }

    [HttpGet("me")]
    public async Task<Result<UserDto>> GetMe()
    {
        return await _userService.GetMe();
    }
}