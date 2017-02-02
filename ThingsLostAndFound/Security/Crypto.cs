using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ThingsLostAndFound.Security
{
    public class Crypto
    {
        public static string getSalt()
        {
            var random = new RNGCryptoServiceProvider();
            int max_length = 16;
            byte[] salt = new byte[max_length];
            random.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string Hash(string value, string salt)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value+salt)));
        }

        private static Random randomPass = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[randomPass.Next(s.Length)]).ToArray());
        }
    }
}