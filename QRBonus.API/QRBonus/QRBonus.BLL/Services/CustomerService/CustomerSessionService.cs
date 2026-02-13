using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using QRBonus.BLL.Helpers;
using QRBonus.BLL.Mappers;
using QRBonus.BLL.Models;
using QRBonus.BLL.Services.ErrorService;
using QRBonus.BLL.Services.SmsService;
using QRBonus.DAL;
using QRBonus.DAL.Models;
using QRBonus.DTO.CustomerDtos;
using QRBonus.DTO.UserDtos;

namespace QRBonus.BLL.Services.CustomerService
{
    public class CustomerSessionService : ICustomerSessionService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IRegionConfiguration _regionConfiguration;
        private readonly TwilioSettings _twilioSettings;
        private readonly MsgGeSettings _msgGeSettings;
        private readonly NikitaSettings _nikitaSettings;
        private readonly IErrorService _errorService;
        private readonly ISmsService _nikitaService;
        private readonly ISmsService _msgGeService;
        private readonly ISmsService _twilioService;

        public CustomerSessionService(AppDbContext db,
            IHttpContextAccessor httpContext,
            ICustomerService customerService,
            IOptions<TwilioSettings> twilioSettings,
            IOptions<NikitaSettings> nikitaSettings,
            IRegionConfiguration regionConfiguration,
            IErrorService errorService,
            IOptions<MsgGeSettings> msgGeSettings,
            [FromKeyedServices("nikita")] ISmsService nikitaService,
            [FromKeyedServices("msgGe")] ISmsService msgGeService,
            [FromKeyedServices("twilio")] ISmsService twilioService)
        {
            _db = db;
            _httpContext = httpContext;
            _customerService = customerService;
            _regionConfiguration = regionConfiguration;
            _errorService = errorService;
            _twilioSettings = twilioSettings.Value;
            _nikitaSettings = nikitaSettings.Value;
            _msgGeSettings = msgGeSettings.Value;
            _nikitaService = nikitaService;
            _msgGeService = msgGeService;
            _twilioService = twilioService;
        }
        public Customer CurrentCustomer { get; private set; }

        public async Task<CustomerDto?> GetByToken(string token)
        {
            var customerSession = await _db.CustomerSessions
                .IgnoreQueryFilters()
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Token == token && !x.IsExpired);

            if (customerSession is null)
            {
                return null;
            }

            CurrentCustomer = customerSession.Customer!;
            _regionConfiguration.RegionId = CurrentCustomer.RegionId;

            return CurrentCustomer.MapToCustomerDto();
        }

        public async Task<Result> Login(CustomerLoginDto dto)
        {
            var customer = await _db.Customers.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

            if (customer is null)
            {
                var areaCode = (await _db.Regions.FirstOrDefaultAsync(x => x.Id == dto.RegionId))?.AreaCode;
                if(areaCode is null || !dto.PhoneNumber.StartsWith(areaCode))
                {
                    return Result.Error("Taro Cards tells that phone number and region not matching");
                }

                customer = new Customer
                {
                    PhoneNumber = dto.PhoneNumber,
                    RegionId = dto.RegionId
                };

                _db.Customers.Add(customer);
            }

            if (customer.IsDeleted)
            {
                customer.IsDeleted = false;
            }

            await _db.SaveChangesAsync();

            var sending = await SendVerificationCode(dto.PhoneNumber);

            if (!sending.IsSuccess)
            {
                return Result.Error();
            }

            return Result.Success();
        }

        public async Task<Result> SendVerificationCode(string phoneNumber)
        {
            var customer = await _db.Customers.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            if (customer is null || (customer.PhoneNumber != phoneNumber && await _db.Customers.AnyAsync(x => x.PhoneNumber == phoneNumber)))
            {
                return Result.Error();
            }
            customer.VerificationCode = GenerateVerificationCode();

            var sendCode = customer.PhoneNumber switch
            {
                { } when customer.PhoneNumber.StartsWith("+374") => await _nikitaService.SendCode(customer.PhoneNumber!, customer.VerificationCode),
                { } when customer.PhoneNumber.StartsWith("+995") => await _msgGeService.SendCode(customer.PhoneNumber!, customer.VerificationCode),
                _ => await _twilioService.SendCode(customer.PhoneNumber!, customer.VerificationCode),
            };

            if (!sendCode.IsSuccess)
            {
                return sendCode;
            }

            await _db.SaveChangesAsync();
            return Result.Success();
        }

        private string GenerateVerificationCode()
        {
            if (_twilioSettings.IsTestMode || _msgGeSettings.IsTestMode || _nikitaSettings.IsTestMode)
            {
                return "000000";
            }

            Random r = new Random();
            int randNum = r.Next(1000000);
            string code = randNum.ToString("D6");

            return code;
        }
        public async Task<Result<CustomerSessionDto>> VerifyCustomer(CustomerVerificationDto dto)
        {
            var customer = await _db.Customers.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

            if (customer is null)
            {
                return Result.Error();
            }

            if (dto.Code != customer.VerificationCode)
            {
                return Result.Error();
            }

            var customerSessions = await _db.CustomerSessions
                .Where(x => x.CustomerId == customer.Id && !x.IsExpired)
                .ToListAsync();

            foreach(var cs in customerSessions)
            {
                cs.IsExpired = true;
            }

            customer.IsVerified = true;
            customer.VerificationCode = null;

            await _db.SaveChangesAsync();

            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            var session = new CustomerSession
            {
                Token = token,
                CustomerId = customer.Id
            };

            _db.CustomerSessions.Add(session);

            await _db.SaveChangesAsync();

            var customerSession = await _db.CustomerSessions.IgnoreQueryFilters().Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == session.Id);

            return customerSession!.MapToCustomerSessionDto();

        }

        public async Task<Result> LogOut()
        {
            _httpContext.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var token);

            var session = await _db.CustomerSessions.FirstOrDefaultAsync(x => x.Token == token.ToString());

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
