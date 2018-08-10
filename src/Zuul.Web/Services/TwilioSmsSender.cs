using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Zuul.Web.Models;

namespace Zuul.Web.Services
{
    public class TwilioSmsSender : ISmsSender
    {
        public AppSettings AppSettings { get; }
        public TwilioSmsSender(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }
        public async Task SendSmsAsync(string number, string message)
        {
            var accountSid = AppSettings.SMS.AccountIdentification;
            var authToken = AppSettings.SMS.AccountPassword;

            TwilioClient.Init(accountSid, authToken);

            await MessageResource.CreateAsync(
                to: new PhoneNumber(number),
                from: new PhoneNumber(AppSettings.SMS.AccountFrom),
                body: message);
        }
    }
}
