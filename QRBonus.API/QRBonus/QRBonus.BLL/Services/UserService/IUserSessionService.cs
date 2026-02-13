using Ardalis.Result;
using QRBonus.DAL.Models;
using QRBonus.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Services.UserService
{
    public interface IUserSessionService
    {
        User CurrentUser { get; }
        Task<Result<UserSessionDto>> Login(LoginDto dto);
        Task<Result> LogOut();
        Task<UserDto?> GetByToken(string token);
    }
}
