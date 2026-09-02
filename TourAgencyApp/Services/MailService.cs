using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

namespace TourAgencyApp.Services
{
    public static class MailService
    {
        const string SMTP_HOST = "smtp.mail.ru";
        const int SMTP_PORT = 465; // 465 or 587
        const string EMAIL_ADDRESS = "sttrebery@mail.ru";
        const string APP_PASSWORD = "D2epligvWS874QYwganS";

        //метод для отправки письма с кодом подтверждения
        public static async Task SendConfirmationEmail(string code, string email_to)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Email Confirmation Example", EMAIL_ADDRESS));
            message.To.Add(new MailboxAddress("receiver", email_to)); //адрес получателя
            message.Subject = "Confirm your email";

            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format($"<h1>Ваш код подтверждения:</h1><p style=\"text-align: center; font-size: xx-large; font-weight: bold;\">{code}</p><p>Чтобы завершить регистрацию отправьте ответное письмо на этот адрес</p>");
            message.Body = builder.ToMessageBody();

            using SmtpClient client = new SmtpClient();
            await client.ConnectAsync(SMTP_HOST, SMTP_PORT);
            await client.AuthenticateAsync(EMAIL_ADDRESS, APP_PASSWORD);
            await client.SendAsync(message);
        }

    }
}
