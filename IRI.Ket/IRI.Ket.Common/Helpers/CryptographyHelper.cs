using System;
using System.Collections.Generic;
using System.IO;
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




        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        public static string GetRandomDataBase64url(uint length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[length];
                rng.GetBytes(bytes);
                return GetBase64urlEncodeNoPadding(bytes);
            }
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputStirng"></param>
        /// <returns></returns>
        public static byte[] GetSha256(string inputStirng)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputStirng);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetBase64urlEncodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }
         
        //static byte[] EncryptAes(string plainText, byte[] Key, byte[] IV)
        //{
        //    byte[] encrypted;
        //    // Create a new AesManaged.    
        //    using (AesManaged aes = new AesManaged())
        //    {
        //        // Create encryptor    
        //        ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
        //        // Create MemoryStream    
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            // Create crypto stream using the CryptoStream class. This class is the key to encryption    
        //            // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
        //            // to encrypt    
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //            {
        //                // Create StreamWriter and write data to a stream    
        //                using (StreamWriter sw = new StreamWriter(cs))
        //                    sw.Write(plainText);
        //                encrypted = ms.ToArray();
        //            }
        //        }
        //    }
        //    // Return encrypted data    
        //    return encrypted;
        //}

        //static string DecryptAes(byte[] cipherText, byte[] Key, byte[] IV)
        //{
        //    string plaintext = null;
        //    // Create AesManaged    
        //    using (AesManaged aes = new AesManaged())
        //    {
        //        // Create a decryptor    
        //        ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
        //        // Create the streams used for decryption.    
        //        using (MemoryStream ms = new MemoryStream(cipherText))
        //        {
        //            // Create crypto stream    
        //            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        //            {
        //                // Read crypto stream    
        //                using (StreamReader reader = new StreamReader(cs))
        //                    plaintext = reader.ReadToEnd();
        //            }
        //        }
        //    }
        //    return plaintext;
        //}
    }

    public class SimplerAES
    {
        private static byte[] _key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };

        // a hardcoded IV should not be used for production AES-CBC code
        // IVs should be unpredictable per ciphertext
        private static byte[] _vector; //= __Replace_Me_({ 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 });

        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public SimplerAES(byte[] vector)
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(_key, vector);
            decryptor = rm.CreateDecryptor(_key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
