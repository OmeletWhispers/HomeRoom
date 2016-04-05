using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.Helpers
{
    public static class Helpers
    {
        public static string GenerateRandomPassword(int passwordLength)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            var passwordChars = new char[passwordLength];

            var randomPassword = new Random();

            for (var i = 0; i < passwordLength; i++)
            {
                passwordChars[i] = allowedChars[randomPassword.Next(0, allowedChars.Length)];
            }

            return new string(passwordChars);
        }
    }
}
