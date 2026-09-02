using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TourAgencyApp.Services
{
    public static class HashService
    {
        public static string HashPassword(string password)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// if input's hash equals to hash, return true, otherwise - false
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CompareWithHash(string hash, string input)
        {
            string temp_hash = HashPassword(input);
            if(temp_hash == hash)
                return true;
            else
                return false;
        }
    }
}
