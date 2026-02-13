using Ardalis.Result;
using InnLine.BLL.Constants;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.DAL.Models;
using QRBonus.DAL;
using QRBonus.DTO.UserDtos;
using QRBonus.DTO;
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
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IErrorService _errorService;
        private readonly IUserSessionService _userSessionService;

        public UserService(AppDbContext db, IErrorService errorService, IUserSessionService userSessionService)
        {
            _db = db;
            _errorService = errorService;
            _userSessionService = userSessionService;
        }

        public async Task<Result<BaseDto>> Add(AddUserDto addUserDto)
        {
            if (await _db.Users.AnyAsync(x => x.UserName == addUserDto.UserName))
            {
                return Result.Error(await _errorService.GetErrorName(ErrorConstants.DuplicateItem));
            }

            var user = new User
            {
                FirstName = addUserDto.FirstName,
                LastName = addUserDto.LastName,
                UserName = addUserDto.UserName.ToLower().Trim(),
                PasswordHash = Crypto.HashPassword(addUserDto.Password),
                Role = addUserDto.Role,
            };

            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return new BaseDto
            {
                Id = user.Id,
            };
        }

        public async Task<Result> Delete(long id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user is null)
            {
                return Result.NotFound();
            }

            user.IsDeleted = true;

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<PagedResult<List<UserDto>>> GetAll(UserFilter filter)
        {
            var query = _db.Users.AsQueryable();

            var entities = await filter.FilterObjects(query).ToListAsync();

            return new PagedResult<List<UserDto>>(await filter.GetPagedInfoAsync(query), entities.MapToUserDtos());
        }

        public async Task<Result<UserDto>> GetById(long id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user is null)
            {
                return Result.NotFound();
            }

            var userDto = user.MapToUserDto();

            return userDto;
        }

        public async Task<Result<UserDto>> GetMe()
        {
            var user = _userSessionService.CurrentUser;

            return await GetById(user.Id);
        }

        public async Task<Result> Update(long id, UpdateUserDto updateUserDto)
        {
            var user = await _db.Users.FindAsync(id);

            if (user is null)
            {
                return Result.NotFound();
            }

            if (await _db.Users
                .Where(x => x.Id != user.Id)
                .AnyAsync(x => x.UserName == updateUserDto.UserName))
            {
                return Result.Error(await _errorService.GetErrorName(ErrorConstants.DuplicateItem));
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.UserName = updateUserDto.UserName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
            {
                user.PasswordHash = Crypto.HashPassword(updateUserDto.Password);
            }

            await _db.SaveChangesAsync();

            return Result.Success();
        }
    }
}