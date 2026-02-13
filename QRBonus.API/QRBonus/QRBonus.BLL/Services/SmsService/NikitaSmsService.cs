using Ardalis.Result;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QRBonus.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using QRBonus.DTO.NIkitaSmsDtos;
using Twilio.TwiML.Voice;
using QRCoder.Extensions;

namespace QRBonus.BLL.Services.SmsService;

public class NikitaSmsService : ISmsService
{
    private readonly NikitaSettings _nikitaSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public NikitaSmsService(IOptions<NikitaSettings> nikitaSettings, IHttpClientFactory httpClientFactory)
    {
        _nikitaSettings = nikitaSettings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> SendCode(string phoneNumber, string verificationCode)
    {
        if (_nikitaSettings.IsTestMode)
        {
            return Result.Success();
        }

        var smsData = new SmsRequest()
        {
            Messages = new List<DTO.NIkitaSmsDtos.Message>()
                {
                    new DTO.NIkitaSmsDtos.Message()
                    {
                        Sms = new DTO.NIkitaSmsDtos.Sms()
                        {
                            Originator = _nikitaSettings.Originator,
                            Content = new Content()
                            {
                                Text = verificationCode
                            },
                        },
                        Priority = _nikitaSettings.Priority,
                        MessageId = $"{phoneNumber}{Guid.NewGuid}",
                        Recipient = phoneNumber
                    }
                }
        };

        var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_nikitaSettings.Login}:{_nikitaSettings.Pass}"));
        var message = new HttpRequestMessage(HttpMethod.Post, _nikitaSettings.Url);
        message.Content = new StringContent(JsonConvert.SerializeObject(smsData), Encoding.UTF8, "application/json");
        message.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

        using (var client = _httpClientFactory.CreateClient())
        {

            var result = await client.SendAsync(message);

            if (result.IsSuccessStatusCode)
            {
                return Result.Success();
            }
   
            else return Result.Error();
        }
               
    }
}