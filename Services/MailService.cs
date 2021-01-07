using System.IO;
using System.Threading.Tasks;
using ems.Models;
using ems.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ems.Services
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSetting;

        public MailService(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;
        }
        public async Task SendMailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            foreach (var toAddress in mailRequest.ToEmails)
            {
                email.To.Add(MailboxAddress.Parse(toAddress));
            }
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        byte[] fileBytes;
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            fileBytes = stream.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, data: fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}