using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRBonus.BLL.Services.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QRBonus.BLL.Helpers
{
    public class CustomerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ICustomerSessionService _sessionService;
        public CustomerAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger,
            UrlEncoder encoder, 
            ISystemClock clock, 
            ICustomerSessionService sessionService) : base(options, logger, encoder, clock)
        {
            _sessionService = sessionService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;

            if (Context.Request.Path.StartsWithSegments("/notificationHub"))
            {
                token = Context.Request.Query["access_token"];
            }
            else
            {
                token = Request.Headers["Authorization"].ToString();
            }

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Authorization not provided");
            }


            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues) && token is null)
            {
                return AuthenticateResult.NoResult();
            }

            token ??= authorizationHeaderValues.FirstOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.NoResult();
            }

            var user = await _sessionService.GetByToken(token);

            if (user is null)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.PhoneNumber),
            new Claim(ClaimTypes.Name, string.Concat(user.FirstName,user.LastName))
        };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
