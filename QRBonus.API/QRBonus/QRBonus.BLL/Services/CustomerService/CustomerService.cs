using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QRBonus.BLL.Services.SmsService;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO;
using QRBonus.DTO.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Services.PushNotificationService;
using QRBonus.Shared.Enums;
using static QRCoder.PayloadGenerator;

namespace QRBonus.BLL.Services.CustomerService
{
    public class CustomerService : CustomerBaseService, ICustomerService
    {
        private readonly IPushNotificationService _pushNotificationService;

        public CustomerService(AppDbContext db, IPushNotificationService pushNotificationService) : base(db)
        {
            _pushNotificationService = pushNotificationService;
        }

        public async Task<Result> Delete(long id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            
            if (customer is null)
            {
                return Result.NotFound();
            }

            customer.IsDeleted = true;
            customer.IsVerified = false;

            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateToken(long userId, UpdateCustomerFireBaseTokenDto fireBaseToken,
            long currentCustomerRegionId)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == userId);
            
            if (customer is null)
            {
                return Result.NotFound();
            }
            
            customer.Token = fireBaseToken.Token;
            customer.DeviceType = fireBaseToken.DeviceType;
            await _pushNotificationService.SubscribeUsersToTopic(new List<string> { customer.Token }, NotificationTopicConstants.AllUsers + currentCustomerRegionId);

            switch (customer.DeviceType)
            {
                case DeviceType.Android:
                    await _pushNotificationService.SubscribeUsersToTopic(
                        new List<string> { customer.Token }, NotificationTopicConstants.AndroidUsers + currentCustomerRegionId);
                    break;
                case DeviceType.Ios:
                    await _pushNotificationService.SubscribeUsersToTopic(
                        new List<string> { customer.Token }, NotificationTopicConstants.IosUsers + currentCustomerRegionId);
                    break;
                default:
                    break;
            }
            
            await _db.SaveChangesAsync();
            
            return Result.Success();
        }

        public async Task<Result> Update(long id, CustomerDto dto)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer is null)
            {
                return Result.Error();
            }

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;

            if (customer.PhoneNumber != dto.PhoneNumber)
            {
                if(await _db.Customers.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber))
                {
                    return Result.Error();
                }
                
                customer.PhoneNumber = dto.PhoneNumber;
                customer.IsVerified = false;
            }
            await _db.SaveChangesAsync();

            return Result.Success();
        }

    }
}