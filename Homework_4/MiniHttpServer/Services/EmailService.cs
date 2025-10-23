using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Services
{
    public static class EmailService
    {
        public static void SendEmail(string email, string subject, string message)
        {
            var gmailSettings = new EmailServiceSettings()
            {
                Name = "Gmail",
                Host = "smtp.gmail.com",
                Sender = "testemailsendler@gmail.com",
                Port = 587,
                EnableSSL = true,
                UserName = "testemailsendler@gmail.com",
                Password = "aoax cdjz xsga vxnu"
            };

            var yandexSettings = new EmailServiceSettings()
            {
                Name = "Yandex",
                Host = "smtp.yandex.ru",
                Sender= "dimitry.rezvyakov@yandex.ru",
                Port = 587,
                EnableSSL = true,
                UserName = "dimitry.rezvyakov",
                Password = "rwsrfqggcuskogpg"
            };

            var googleSmtp = CreateSmtp(email, subject, message, gmailSettings);

            var yandexSmtp = CreateSmtp(email, subject, message, yandexSettings);

            Action[] clients = new[] { googleSmtp, yandexSmtp };

            foreach (var client in clients)
            {
                try
                {
                    client.Invoke();
                    Console.WriteLine("Сообщение отправлено");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка отправки");
                }
            }
        }

        public static Action CreateSmtp(string email, string subject, string message, EmailServiceSettings settings)
        {
            MailAddress from = new MailAddress(settings.Sender, settings.Name);

            MailAddress to = new MailAddress(email);

            MailMessage m = new MailMessage(from, to);

            m.Subject = subject;

            m.Body = $"<h1>Your login is: {email.ToString()}<h1/><br/>" +
                       $"<h1>Your password: {message.ToString()}<h1/>";

            m.IsBodyHtml = true;

            m.Attachments.Add(new Attachment("file.zip"));

            SmtpClient smtp = new SmtpClient(settings.Host, settings.Port);

            smtp.Credentials = new NetworkCredential(settings.UserName, settings.Password);
            smtp.EnableSsl = settings.EnableSSL;

            return () =>
            {
                smtp.Send(m);
            };
        }
    }

    public class EmailServiceSettings()
    {
        public string Name { get; set; }
        public string Host {  get; set; }
        public string Sender { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
