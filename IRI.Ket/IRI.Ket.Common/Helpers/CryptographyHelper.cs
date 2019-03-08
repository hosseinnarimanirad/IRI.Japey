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


        #region SymmetricCryptography > Rijndael

        public static string AesEncrypt(string value, byte[] key, byte[] iv)
        {
            using (RijndaelManaged rm = new RijndaelManaged())
            {
                using (var encryptor = rm.CreateEncryptor(key, iv))
                {
                    var original = System.Text.Encoding.UTF8.GetBytes(value);

                    return Convert.ToBase64String(Transform(original, encryptor));
                }
            }
        }

        public static string AesDecrypt(string base64String, byte[] key, byte[] iv)
        {
            using (RijndaelManaged rm = new RijndaelManaged())
            {
                using (var decryptor = rm.CreateDecryptor(key, iv))
                {
                    var original = Convert.FromBase64String(base64String);

                    return System.Text.Encoding.UTF8.GetString(Transform(original, decryptor));
                }
            }
        }

        #endregion


        #region AsymmetricCryptography > RSA

        public static string RsaEncrypt(byte[] original, string base46PublicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                { 
                    rsa.FromXmlString(base46PublicKey.Base64ToNormalString());

                    return Convert.ToBase64String(rsa.Encrypt(original, true));
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string RsaEncrypt(string value, string base46PublicKey)
        {
            var originalByte = Encoding.UTF8.GetBytes(value);

            return RsaEncrypt(originalByte, base46PublicKey);

            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            //{
            //    try
            //    {
            //        var originalByte = Encoding.UTF8.GetBytes(value);

            //        rsa.FromXmlString(base46PublicKey.Base64ToNormalString());

            //        return Convert.ToBase64String(rsa.Encrypt(originalByte, true));
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally
            //    {
            //        rsa.PersistKeyInCsp = false;
            //    }
            //}
        }


        public static string RsaDecrypt(byte[] encoded, string base64PrivateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    rsa.FromXmlString(base64PrivateKey.Base64ToNormalString());

                    return Encoding.UTF8.GetString(rsa.Decrypt(encoded, true));
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string RsaDecrypt(string base64EncodedString, string base64PrivateKey)
        {
            var encodedBytes = Convert.FromBase64String(base64EncodedString);

            return RsaDecrypt(encodedBytes, base64PrivateKey);

            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            //{
            //    try
            //    {
            //        var encodedBytes = Convert.FromBase64String(base64EncodedString);

            //        rsa.FromXmlString(base64PrivateKey.Base64ToNormalString());

            //        return Encoding.UTF8.GetString(rsa.Decrypt(encodedBytes, true));
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        rsa.PersistKeyInCsp = false;
            //    }
            //}
        }


        public static void test()
        {
            var csp = new RSACryptoServiceProvider(2048);

            //how to get the private key
            var privKey = csp.ExportParameters(true);

            //and the public key ...
            var pubKey = csp.ExportParameters(false);

            //converting the public key into a string representation
            string pubKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, pubKey);
                //get the string from the stream
                pubKeyString = sw.ToString();
            }

            //converting it back
            {
                //get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }

            //conversion for the private key is no black magic either ... omitted

            //we have a public key ... let's get a new csp and load that key
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);

            //we need some data to encrypt
            var plainTextData = "foobar";

            //for encryption, always handle bytes...
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            //we might want a string representation of our cypher text... base64 will do
            var cypherText = Convert.ToBase64String(bytesCypherText);


            /*
             * some transmission / storage / retrieval
             * 
             * and we want to decrypt our cypherText
             */

            //first, get our bytes back from the base64 string ...
            bytesCypherText = Convert.FromBase64String(cypherText);

            //we want to decrypt, therefore we need a csp and load our private key
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            //decrypt and strip pkcs#1.5 padding
            bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
        }
         

        //public static string Encryption(string strText)
        //{
        //    var publicKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        //    var testData = Encoding.UTF8.GetBytes(strText);

        //    using (var rsa = new RSACryptoServiceProvider(1024))
        //    {
        //        try
        //        {
        //            // client encrypting data with public key issued by server                    
        //            rsa.FromXmlString(publicKey.ToString());

        //            var encryptedData = rsa.Encrypt(testData, true);

        //            var base64Encrypted = Convert.ToBase64String(encryptedData);

        //            return base64Encrypted;
        //        }
        //        finally
        //        {
        //            rsa.PersistKeyInCsp = false;
        //        }
        //    }
        //}

        //public static string Decryption(string strText)
        //{
        //    var privateKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent><P>/aULPE6jd5IkwtWXmReyMUhmI/nfwfkQSyl7tsg2PKdpcxk4mpPZUdEQhHQLvE84w2DhTyYkPHCtq/mMKE3MHw==</P><Q>3WV46X9Arg2l9cxb67KVlNVXyCqc/w+LWt/tbhLJvV2xCF/0rWKPsBJ9MC6cquaqNPxWWEav8RAVbmmGrJt51Q==</Q><DP>8TuZFgBMpBoQcGUoS2goB4st6aVq1FcG0hVgHhUI0GMAfYFNPmbDV3cY2IBt8Oj/uYJYhyhlaj5YTqmGTYbATQ==</DP><DQ>FIoVbZQgrAUYIHWVEYi/187zFd7eMct/Yi7kGBImJStMATrluDAspGkStCWe4zwDDmdam1XzfKnBUzz3AYxrAQ==</DQ><InverseQ>QPU3Tmt8nznSgYZ+5jUo9E0SfjiTu435ihANiHqqjasaUNvOHKumqzuBZ8NRtkUhS6dsOEb8A2ODvy7KswUxyA==</InverseQ><D>cgoRoAUpSVfHMdYXW9nA3dfX75dIamZnwPtFHq80ttagbIe4ToYYCcyUz5NElhiNQSESgS5uCgNWqWXt5PnPu4XmCXx6utco1UVH8HGLahzbAnSy6Cj3iUIQ7Gj+9gQ7PkC434HTtHazmxVgIR5l56ZjoQ8yGNCPZnsdYEmhJWk=</D></RSAKeyValue>";

        //    var testData = Encoding.UTF8.GetBytes(strText);

        //    using (var rsa = new RSACryptoServiceProvider(1024))
        //    {
        //        try
        //        {
        //            var base64Encrypted = strText;

        //            // server decrypting data with private key                    
        //            rsa.FromXmlString(privateKey);

        //            var resultBytes = Convert.FromBase64String(base64Encrypted);
        //            var decryptedBytes = rsa.Decrypt(resultBytes, true);
        //            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
        //            return decryptedData.ToString();
        //        }
        //        finally
        //        {
        //            rsa.PersistKeyInCsp = false;
        //        }
        //    }
        //}
         

        static public byte[] RsaEncrypt(byte[] value, RSAParameters rsaKey, bool doOAEPPadding = true)
        {
            try
            {
                byte[] encryptedData;

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaKey);

                    encryptedData = rsa.Encrypt(value, doOAEPPadding);
                }

                return encryptedData;
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }

        static public byte[] RsaDecrypt(byte[] value, RSAParameters rsaKey, bool doOAEPPadding)
        {
            try
            {
                byte[] decryptedData;

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaKey);

                    decryptedData = rsa.Decrypt(value, doOAEPPadding);
                }

                return decryptedData;
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }

        #endregion


        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();

            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }

            return stream.ToArray();
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

    //public class SimplerAES
    //{
    //    private static byte[] _key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };

    //    // a hardcoded IV should not be used for production AES-CBC code
    //    // IVs should be unpredictable per ciphertext
    //    private static byte[] _vector; //= __Replace_Me_({ 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 });

    //    private ICryptoTransform encryptor, decryptor;
    //    private UTF8Encoding encoder;


    //    public SimplerAES()
    //    {
    //        RijndaelManaged rm = new RijndaelManaged();
    //        rm.GenerateIV();
    //        _vector = rm.IV;
    //        //_vector = secret;
    //        encryptor = rm.CreateEncryptor(_key, _vector);
    //        decryptor = rm.CreateDecryptor(_key, _vector);
    //        encoder = new UTF8Encoding();
    //    }

    //    public byte[] GetIV()
    //    {
    //        return _vector;
    //    }

    //    public string Encrypt(string unencrypted)
    //    {
    //        return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
    //    }

    //    public string Decrypt(string encrypted)
    //    {
    //        return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
    //    }

    //    public byte[] Encrypt(byte[] buffer)
    //    {
    //        return Transform(buffer, encryptor);
    //    }

    //    public byte[] Decrypt(byte[] buffer)
    //    {
    //        return Transform(buffer, decryptor);
    //    }

    //    protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
    //    {
    //        MemoryStream stream = new MemoryStream();
    //        using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
    //        {
    //            cs.Write(buffer, 0, buffer.Length);
    //        }
    //        return stream.ToArray();
    //    }
    //}
}
