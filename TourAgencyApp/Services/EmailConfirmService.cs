using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TourAgencyApp.ViewModels;
using TourAgencyApp.Views;

namespace TourAgencyApp.Services
{
    public class EmailConfirmService
    {
        static Random random = new Random();
        static readonly string email_pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email, email_pattern, RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string GenerateCode()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(random.Next(0, 10).ToString());
            }
            return builder.ToString();
        }

        public static async Task<bool> EmailConfirmAsync(string Email)
        {
            string code = EmailConfirmService.GenerateCode();
            Window email_confirm = new EmailConfirmView() { DataContext = new EmailConfirmViewModel(code) };

            await MailService.SendConfirmationEmail(code, Email);

            email_confirm.ShowDialog();
            if ((email_confirm.DataContext as EmailConfirmViewModel)!.Success)
            {
                return true;
            }
            else return false;
        }
    }
}
