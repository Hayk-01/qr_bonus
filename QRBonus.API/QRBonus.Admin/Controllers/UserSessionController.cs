using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Services.UserService;
using QRBonus.DTO.UserDtos;
using QRBonus.Shared.Enums;

namespace QRBonus.Admin.Controllers;

public class UserSessionController : ApiControllerBase
{
    private readonly IUserSessionService _userSessionService;

    public UserSessionController(IUserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<Result<UserSessionDto>> Login(LoginDto dto)
    {
        return await _userSessionService.Login(dto);
    }

    [AllowAnonymous]
    [Authorize(Roles = $"{nameof(UserRole.Courier)}, {nameof(UserRole.Admin)}")]
    [HttpPost("logout")]
    public async Task<Result> LogOut()
    {
        return await _userSessionService.LogOut();
    }

}
