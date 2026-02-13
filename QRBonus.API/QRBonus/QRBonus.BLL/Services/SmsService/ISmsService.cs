using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace QRBonus.BLL.Services.SmsService
{
    public interface ISmsService
    {
        Task<Result> SendCode(string phoneNumber, string verificationCode);
    }
}
