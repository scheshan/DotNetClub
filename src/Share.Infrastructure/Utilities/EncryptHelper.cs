using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Share.Infrastructure.Utilities
{
    public sealed class EncryptHelper
    {
        private static MD5 _md5 = MD5.Create();

        public static string EncryptMD5(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            byte[] inputData = Encoding.UTF8.GetBytes(input);
            byte[] computeData = _md5.ComputeHash(inputData);

            return BitConverter.ToString(computeData).Replace("-", "").ToUpper();
        }
    }
}
