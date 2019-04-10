using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SenderService
{
    public class SenderManager : Contracts.ISenderManager
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("VDAR.KZ", "info@vdar.kz"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("info@vdar.kz", 25, false);
                await client.AuthenticateAsync("info@vdar.kz", "Pa$$word123");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendSMSAsync(string phone, string subject, string message)
        {
            const string accountSid = "AC01c759192b28af4bc7b7c9e4ebc7d613";
            const string authToken = "50dc60bdd9dda1e47c57954a0b583306";
            TwilioClient.Init(accountSid, authToken);
            string code = "1234";
            var to = new PhoneNumber("+77058756302");
            await MessageResource.CreateAsync(
                to,
                from: new PhoneNumber("+17205065770"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ).  
                body: $"Welcome to VDAR.KZ. Authentication code is: {code} !!");

        }
    }
}
