using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zuul.Web.Models;

namespace Zuul.Web.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        public AppSettings AppSettings { get; }
        public SendGridEmailSender(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new EmailAddress(AppSettings.Email.FromAddress);

            myMessage.AddTo(new EmailAddress(email));

            myMessage.Subject = subject;

            //Add the HTML and Text bodies
            myMessage.HtmlContent = message;
            //myMessage.Text = "Hello World plain text!";

            // Create a Web transport, using API Key
            var transportWeb = new SendGrid.SendGridClient(AppSettings.Email.Username);

            // Send the email.
            await transportWeb.SendEmailAsync(myMessage);
        }
    }
}
