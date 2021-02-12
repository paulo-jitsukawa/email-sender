using Jitsukawa.EmailSender.Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Jitsukawa.EmailSender.Api.Services
{
    public class EmailService
    {
        private readonly AppSettings settings;

        public EmailService(IOptions<AppSettings> configuration)
        {
            settings = configuration.Value;
        }

        public async Task<string[]> Send(EmailMessage e)
        {
            var fails = new List<string>();

            var client = new SmtpClient(settings.SMTP, settings.Port)
            {
                EnableSsl = settings.SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            if (!string.IsNullOrWhiteSpace(settings.User))
            {
                client.Credentials = new NetworkCredential(settings.User, settings.Password);
            }

            foreach (var recipient in e.Recipients)
            {
                try
                {
                    var email = new MailMessage(settings.Sender, recipient)
                    {
                        Subject = e.Subject,
                        Body = e.Body,
                        IsBodyHtml = e.HTML
                    };

                    await client.SendMailAsync(email);
                }
                catch (Exception e1)
                {
                    fails.Add(recipient);
                }
            }

            return fails.ToArray();
        }
    }
}
