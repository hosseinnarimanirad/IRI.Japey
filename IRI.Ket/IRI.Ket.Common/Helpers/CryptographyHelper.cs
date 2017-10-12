using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class CryptographyHelper
    {
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("X2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }
        }

        public static string GetMd5Hash(string input, string salt)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input + salt));

                return Convert.ToBase64String(data);
            }
        }

        public static string SimpleEncrypt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value.ToString()));
        }

        public static string SimpleDecrypt(string hashedValue)
        {
            if (string.IsNullOrWhiteSpace(hashedValue))
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(hashedValue));
        }

    }
}
