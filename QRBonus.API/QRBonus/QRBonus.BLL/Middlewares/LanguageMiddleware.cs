using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QRBonus.BLL.Enums;
using QRBonus.BLL.Services.CustomerService;
using QRBonus.BLL.Services.UserService;
using QRBonus.DAL;
using System.Reflection;

namespace QRBonus.BLL.Middlewares
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate next;

        public LanguageMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var languageConfiguration = context.RequestServices.GetRequiredService<LanguageConfiguration>();

            languageConfiguration.LanguageId = (int)LanguageEnum.English;
            
            var acceptLanguage = context.Request.Headers["Lang-Code"].ToString();

            if (!string.IsNullOrWhiteSpace(acceptLanguage))
            {
                if (int.TryParse(acceptLanguage, out var lang))
                {
                    languageConfiguration.LanguageId = lang;
                }                    
                else
                {
                    languageConfiguration.LanguageId = acceptLanguage switch
                    {
                        "ge" => (int)LanguageEnum.Georgian,
                        _ => (int)LanguageEnum.English
                    };
                }
            }
            await next(context);
        }
    }
}
