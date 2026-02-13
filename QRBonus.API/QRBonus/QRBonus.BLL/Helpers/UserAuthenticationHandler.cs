using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRBonus.BLL.Services.UserService;
using QRBonus.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QRBonus.BLL.Helpers
{
    public class UserAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserSessionService _userSessionService;

        public UserAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IUserSessionService userSessionService) : base(options, logger, encoder, clock)
        {
            _userSessionService = userSessionService;
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

            var user = await _userSessionService.GetByToken(token);

            if (user is null)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.UserName),
            new Claim(ClaimTypes.Name, string.Concat(user.FirstName,user.LastName)),
            new Claim(ClaimTypes.Role, Enum.GetName<UserRole>(user.Role))
        };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
