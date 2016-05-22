using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetClub.Core.Utility
{
    public sealed class EncryptHelper
    {
        public static string Encrypt(byte[] salt, string input)
        {
            string output = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return output;
        }

        public static string Md5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputData = Encoding.UTF8.GetBytes(input);
                var encryptData = md5.ComputeHash(inputData);

                return BitConverter.ToString(encryptData).Replace("-", "");
            }
        }
    }
}
