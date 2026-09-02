using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgencyApp.Services
{
    //todo: доделать
    public class EmailConfirmService
    {
        readonly Random random = new Random();
        readonly string email_pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        private string GenerateCode()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(random.Next(0, 10).ToString());
            }
            return builder.ToString();
        }
    }
}
