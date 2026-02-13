using Ardalis.Result;
using InnLine.BLL.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QRBonus.BLL.Models;
using QRBonus.DTO.MsgGeGtos;
using QRBonus.DTO.TwilioDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using ZXing.QrCode.Internal;

namespace QRBonus.BLL.Services.SmsService
{
    public class MsgGeSmsService : ISmsService
    {
        private readonly MsgGeSettings _msgGeSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public MsgGeSmsService(IOptions<MsgGeSettings> msgGeSettings, IHttpClientFactory httpClientFactory)
        {
            _msgGeSettings = msgGeSettings.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Result> SendCode(string phoneNumber, string verificationCode)
        {
            if (_msgGeSettings.IsTestMode)
            {
                return Result.Success();
            }

            var client = _httpClientFactory.CreateClient();

            var url = $"http://bi.msg.ge/sendsms.php?to={phoneNumber.Substring(1,12)}&text={verificationCode}&service_id={_msgGeSettings.ServiceId}&client_id={_msgGeSettings.ClientId}&password={_msgGeSettings.Password}&username={_msgGeSettings.Username}&result=json";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("MSG_HEADER", _msgGeSettings.AlternatePassword);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Error();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MsgGeResponse>(responseContent);

            if (result.Code != 0)
            {
                return Result.Error(result.CodeDescription);
            }

            return Result.Success();
        }
    }
}
