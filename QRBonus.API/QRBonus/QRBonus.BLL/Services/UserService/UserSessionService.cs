using Ardalis.Result;
using InnLine.BLL.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL.Models;
using QRBonus.DAL;
using QRBonus.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using QRBonus.BLL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace QRBonus.BLL.Services.UserService
{
    public class UserSessionService : IUserSessionService
    {
        private readonly AppDbContext _db;
        private readonly IErrorService _errorService;
        private readonly IHttpContextAccessor _httpContext;


        public UserSessionService(AppDbContext db, 
            IHttpContextAccessor httpContext, 
            IErrorService errorService)
        {
            _db = db;
            _httpContext = httpContext;
            _errorService = errorService;
        }

        public User CurrentUser { get; private set; }

        public async Task<UserDto?> GetByToken(string token)
        {
            var userSession = await _db.UserSessions
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token && !x.IsExpired);

            if (userSession is null)
            {
                return null;
            }

            CurrentUser = userSession.User!;

            return CurrentUser.MapToUserDto();
        }

        public async Task<Result<UserSessionDto>> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == dto.UserName.ToLower().Replace(" ", ""));

            if (user is null || !Crypto.VerifyHashedPassword(user.PasswordHash, dto.Password))
            {
                return Result.Error(await _errorService.GetErrorName(ErrorConstants.TheUsernameOrPasswordIsIncorrect));
            }

            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            var session = new UserSession
            {
                Token = token,
                UserId = user.Id
            };

            _db.UserSessions.Add(session);

            await _db.SaveChangesAsync();

            var userSession = await _db.UserSessions.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == session.Id);

            return userSession.MapToUserSessionDto();

        }

        public async Task<Result> LogOut()
        {
            _httpContext.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var token);

            var session = await _db.UserSessions.FirstOrDefaultAsync(x => x.Token == token.ToString());

            if (session != null)
            {
                session.IsExpired = true;
                session.IsDeleted = true;

                await _db.SaveChangesAsync();
            }

            return Result.Success();

        }
    }
}