using Ardalis.Result;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QRBonus.BLL.Models;
using QRBonus.DTO.TwilioDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace QRBonus.BLL.Services.SmsService
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioSettings _twilioSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public TwilioSmsService(IOptions<TwilioSettings> twilioSettings, IHttpClientFactory httpClientFactory)
        {
            _twilioSettings = twilioSettings.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Result> SendCode(string phoneNumber, string code)
        {
            if (_twilioSettings.IsTestMode)
            {
                return Result.Success();
            }

            var request = new TwilioSmsRequest
            {
                To = phoneNumber,
                From = _twilioSettings.FromNumber,
                Body = code
            };

            var url = $"https://api.twilio.com/2010-04-01/Accounts/{_twilioSettings.AccountSid}/Messages.json";

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("To", request.To),
                new KeyValuePair<string, string>("From", request.From),
                new KeyValuePair<string, string>("Body", request.Body),
            };
    
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(postData)
            };

            var client = _httpClientFactory.CreateClient();

            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_twilioSettings.AccountSid}:{_twilioSettings.AuthToken}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            var response = await client.SendAsync(httpRequest);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Twilio Error: {content}");
            }

            var responseMessage = JsonConvert.DeserializeObject<TwilioSmsResponse>(content, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return Result.Success();
        }
    }
}
