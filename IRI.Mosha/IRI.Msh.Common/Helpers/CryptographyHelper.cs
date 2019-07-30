using IRI.Ket.Common.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class CryptographyHelper
    {
        //USING UTF8
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

        //USING UTF8
        public static string GetMd5Hash(string input, string salt)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input + salt));

                return Convert.ToBase64String(data);
            }
        }

        //USING ASCII
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        //USING ASCII
        public static string CalculateSha256Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = sha256.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string CalculateHash(string input, HashAlgorithm algorithm)
        {
            // step 1, calculate MD5 hash from input
            //SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = algorithm.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
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
                    RsaHelper.FromXmlString(rsa, base46PublicKey.Base64ToNormalString());
                    //rsa.FromXmlString(base46PublicKey.Base64ToNormalString());

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
                    RsaHelper.FromXmlString(rsa, base64PrivateKey.Base64ToNormalString());
                    //rsa.FromXmlString(base64PrivateKey.Base64ToNormalString());

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

        public static string ToBase64String(string simpleString, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            return Convert.ToBase64String(encoding.GetBytes(simpleString));
        }

        public static string FromBase64String(string base64String, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            return encoding.GetString(Convert.FromBase64String(base64String));
        }
    }

    public static class CryptoRSAHelper
    {
        private static byte[] RSA_OID = { 0x30, 0xD, 0x6, 0x9, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0xD, 0x1, 0x1, 0x1, 0x5, 0x0 }; // Object ID for RSA

        // Corresponding ASN identification bytes
        const byte INTEGER = 0x2;
        const byte SEQUENCE = 0x30;
        const byte BIT_STRING = 0x3;
        const byte OCTET_STRING = 0x4;

        public static string ConvertPublicKey(RSAParameters param)
        {
            List<byte> arrBinaryPublicKey = new List<byte>();

            arrBinaryPublicKey.InsertRange(0, param.Exponent);
            arrBinaryPublicKey.Insert(0, (byte)arrBinaryPublicKey.Count);
            arrBinaryPublicKey.Insert(0, INTEGER);

            arrBinaryPublicKey.InsertRange(0, param.Modulus);
            AppendLength(ref arrBinaryPublicKey, param.Modulus.Length);
            arrBinaryPublicKey.Insert(0, INTEGER);

            AppendLength(ref arrBinaryPublicKey, arrBinaryPublicKey.Count);
            arrBinaryPublicKey.Insert(0, SEQUENCE);

            arrBinaryPublicKey.Insert(0, 0x0); // Add NULL value

            AppendLength(ref arrBinaryPublicKey, arrBinaryPublicKey.Count);

            arrBinaryPublicKey.Insert(0, BIT_STRING);
            arrBinaryPublicKey.InsertRange(0, RSA_OID);

            AppendLength(ref arrBinaryPublicKey, arrBinaryPublicKey.Count);

            arrBinaryPublicKey.Insert(0, SEQUENCE);

            return System.Convert.ToBase64String(arrBinaryPublicKey.ToArray());
        }

        public static string ConvertPrivateKey(RSAParameters param)
        {
            List<byte> arrBinaryPrivateKey = new List<byte>();

            arrBinaryPrivateKey.InsertRange(0, param.InverseQ);
            AppendLength(ref arrBinaryPrivateKey, param.InverseQ.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.DQ);
            AppendLength(ref arrBinaryPrivateKey, param.DQ.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.DP);
            AppendLength(ref arrBinaryPrivateKey, param.DP.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.Q);
            AppendLength(ref arrBinaryPrivateKey, param.Q.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.P);
            AppendLength(ref arrBinaryPrivateKey, param.P.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.D);
            AppendLength(ref arrBinaryPrivateKey, param.D.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.Exponent);
            AppendLength(ref arrBinaryPrivateKey, param.Exponent.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.InsertRange(0, param.Modulus);
            AppendLength(ref arrBinaryPrivateKey, param.Modulus.Length);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            arrBinaryPrivateKey.Insert(0, 0x00);
            AppendLength(ref arrBinaryPrivateKey, 1);
            arrBinaryPrivateKey.Insert(0, INTEGER);

            AppendLength(ref arrBinaryPrivateKey, arrBinaryPrivateKey.Count);
            arrBinaryPrivateKey.Insert(0, SEQUENCE);

            return System.Convert.ToBase64String(arrBinaryPrivateKey.ToArray());
        }

        private static void AppendLength(ref List<byte> arrBinaryData, int nLen)
        {
            if (nLen <= byte.MaxValue)
            {
                arrBinaryData.Insert(0, Convert.ToByte(nLen));
                arrBinaryData.Insert(0, 0x81); //This byte means that the length fits in one byte
            }
            else
            {
                arrBinaryData.Insert(0, Convert.ToByte(nLen % (byte.MaxValue + 1)));
                arrBinaryData.Insert(0, Convert.ToByte(nLen / (byte.MaxValue + 1)));
                arrBinaryData.Insert(0, 0x82); //This byte means that the length fits in two byte
            }
        }


        public static string ConvertPublicKey(string base64PrivateKey)
        {
            return ConvertPublicKey(base64PrivateKey, false);
        }

        public static string ConvertPrivateKey(string base64PrivateKey)
        {
            return ConvertPublicKey(base64PrivateKey, true);
        }

        private static string ConvertPublicKey(string base64PrivateKey, bool includePrivateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    rsa.FromXmlString(base64PrivateKey.Base64ToNormalString());

                    if (includePrivateKey)
                    {
                        return ConvertPrivateKey(rsa.ExportParameters(includePrivateKey));
                    }
                    else
                    {
                        return ConvertPublicKey(rsa.ExportParameters(includePrivateKey));
                    }
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
    }

    #region RSACryptoExtensions

    /*********************************************************************************
 * Copyright (c) 2013, Christian Etter info at christian-etter dot de
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer. 
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *********************************************************************************/


    /// <summary>Extension method for initializing a RSACryptoServiceProvider from PEM data string.</summary>
    public static class RSACryptoServiceProviderExtension
    {
        #region Methods

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a DER public key blob.</summary>
        public static void LoadPublicKeyDER(this RSACryptoServiceProvider provider, byte[] DERData)
        {
            byte[] RSAData = RSACryptoServiceProviderExtension.GetRSAFromDER(DERData);
            byte[] publicKeyBlob = RSACryptoServiceProviderExtension.GetPublicKeyBlobFromRSA(RSAData);
            provider.ImportCspBlob(publicKeyBlob);
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a DER private key blob.</summary>
        public static void LoadPrivateKeyDER(this RSACryptoServiceProvider provider, byte[] DERData)
        {
            byte[] privateKeyBlob = RSACryptoServiceProviderExtension.GetPrivateKeyDER(DERData);
            provider.ImportCspBlob(privateKeyBlob);
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a PEM public key string.</summary>
        public static void LoadPublicKeyPEM(this RSACryptoServiceProvider provider, string sPEM)
        {
            byte[] DERData = RSACryptoServiceProviderExtension.GetDERFromPEM(sPEM);
            RSACryptoServiceProviderExtension.LoadPublicKeyDER(provider, DERData);
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a PEM private key string.</summary>
        public static void LoadPrivateKeyPEM(this RSACryptoServiceProvider provider, string sPEM)
        {
            byte[] DERData = RSACryptoServiceProviderExtension.GetDERFromPEM(sPEM);
            RSACryptoServiceProviderExtension.LoadPrivateKeyDER(provider, DERData);
        }

        /// <summary>Returns a public key blob from an RSA public key.</summary>
        internal static byte[] GetPublicKeyBlobFromRSA(byte[] RSAData)
        {
            byte[] data = null;
            UInt32 dwCertPublicKeyBlobSize = 0;
            if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING,
                new IntPtr((int)CRYPT_OUTPUT_TYPES.RSA_CSP_PUBLICKEYBLOB), RSAData, (UInt32)RSAData.Length, CRYPT_DECODE_FLAGS.NONE,
                data, ref dwCertPublicKeyBlobSize))
            {
                data = new byte[dwCertPublicKeyBlobSize];
                if (!RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING,
                    new IntPtr((int)CRYPT_OUTPUT_TYPES.RSA_CSP_PUBLICKEYBLOB), RSAData, (UInt32)RSAData.Length, CRYPT_DECODE_FLAGS.NONE,
                    data, ref dwCertPublicKeyBlobSize))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return data;
        }

        /// <summary>Converts DER binary format to a CAPI CRYPT_PRIVATE_KEY_INFO structure.</summary>
        internal static byte[] GetPrivateKeyDER(byte[] DERData)
        {
            byte[] data = null;
            UInt32 dwRSAPrivateKeyBlobSize = 0;
            IntPtr pRSAPrivateKeyBlob = IntPtr.Zero;
            if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING, new IntPtr((int)CRYPT_OUTPUT_TYPES.PKCS_RSA_PRIVATE_KEY),
                DERData, (UInt32)DERData.Length, CRYPT_DECODE_FLAGS.NONE, data, ref dwRSAPrivateKeyBlobSize))
            {
                data = new byte[dwRSAPrivateKeyBlobSize];
                if (!RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING, new IntPtr((int)CRYPT_OUTPUT_TYPES.PKCS_RSA_PRIVATE_KEY),
                    DERData, (UInt32)DERData.Length, CRYPT_DECODE_FLAGS.NONE, data, ref dwRSAPrivateKeyBlobSize))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return data;
        }

        /// <summary>Converts DER binary format to a CAPI CERT_PUBLIC_KEY_INFO structure containing an RSA key.</summary>
        internal static byte[] GetRSAFromDER(byte[] DERData)
        {
            byte[] data = null;
            byte[] publicKey = null;
            CERT_PUBLIC_KEY_INFO info;
            UInt32 dwCertPublicKeyInfoSize = 0;
            IntPtr pCertPublicKeyInfo = IntPtr.Zero;
            if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING, new IntPtr((int)CRYPT_OUTPUT_TYPES.X509_PUBLIC_KEY_INFO),
                DERData, (UInt32)DERData.Length, CRYPT_DECODE_FLAGS.NONE, data, ref dwCertPublicKeyInfoSize))
            {
                data = new byte[dwCertPublicKeyInfoSize];
                if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING, new IntPtr((int)CRYPT_OUTPUT_TYPES.X509_PUBLIC_KEY_INFO),
                    DERData, (UInt32)DERData.Length, CRYPT_DECODE_FLAGS.NONE, data, ref dwCertPublicKeyInfoSize))
                {
                    GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    try
                    {
                        info = (CERT_PUBLIC_KEY_INFO)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CERT_PUBLIC_KEY_INFO));
                        publicKey = new byte[info.PublicKey.cbData];
                        Marshal.Copy(info.PublicKey.pbData, publicKey, 0, publicKey.Length);
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                else
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return publicKey;
        }

        /// <summary>Extracts the binary data from a PEM file.</summary>
        internal static byte[] GetDERFromPEM(string sPEM)
        {
            UInt32 dwSkip, dwFlags;
            UInt32 dwBinarySize = 0;

            if (!RSACryptoServiceProviderExtension.CryptStringToBinary(sPEM, (UInt32)sPEM.Length, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, null, ref dwBinarySize, out dwSkip, out dwFlags))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] decodedData = new byte[dwBinarySize];
            if (!RSACryptoServiceProviderExtension.CryptStringToBinary(sPEM, (UInt32)sPEM.Length, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, decodedData, ref dwBinarySize, out dwSkip, out dwFlags))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return decodedData;
        }

        #endregion Methods

        #region P/Invoke Constants

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CRYPT_ACQUIRE_CONTEXT_FLAGS : uint
        {
            CRYPT_NEWKEYSET = 0x8,
            CRYPT_DELETEKEYSET = 0x10,
            CRYPT_MACHINE_KEYSET = 0x20,
            CRYPT_SILENT = 0x40,
            CRYPT_DEFAULT_CONTAINER_OPTIONAL = 0x80,
            CRYPT_VERIFYCONTEXT = 0xF0000000
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CRYPT_PROVIDER_TYPE : uint
        {
            PROV_RSA_FULL = 1
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CRYPT_DECODE_FLAGS : uint
        {
            NONE = 0,
            CRYPT_DECODE_ALLOC_FLAG = 0x8000
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CRYPT_ENCODING_FLAGS : uint
        {
            PKCS_7_ASN_ENCODING = 0x00010000,
            X509_ASN_ENCODING = 0x00000001,
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CRYPT_OUTPUT_TYPES : int
        {
            X509_PUBLIC_KEY_INFO = 8,
            RSA_CSP_PUBLICKEYBLOB = 19,
            PKCS_RSA_PRIVATE_KEY = 43,
            PKCS_PRIVATE_KEY_INFO = 44
        }

        /// <summary>Enumeration derived from Crypto API.</summary>
        internal enum CRYPT_STRING_FLAGS : uint
        {
            CRYPT_STRING_BASE64HEADER = 0,
            CRYPT_STRING_BASE64 = 1,
            CRYPT_STRING_BINARY = 2,
            CRYPT_STRING_BASE64REQUESTHEADER = 3,
            CRYPT_STRING_HEX = 4,
            CRYPT_STRING_HEXASCII = 5,
            CRYPT_STRING_BASE64_ANY = 6,
            CRYPT_STRING_ANY = 7,
            CRYPT_STRING_HEX_ANY = 8,
            CRYPT_STRING_BASE64X509CRLHEADER = 9,
            CRYPT_STRING_HEXADDR = 10,
            CRYPT_STRING_HEXASCIIADDR = 11,
            CRYPT_STRING_HEXRAW = 12,
            CRYPT_STRING_NOCRLF = 0x40000000,
            CRYPT_STRING_NOCR = 0x80000000
        }

        #endregion P/Invoke Constants

        #region P/Invoke Structures

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct CRYPT_OBJID_BLOB
        {
            internal UInt32 cbData;
            internal IntPtr pbData;
        }

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct CRYPT_ALGORITHM_IDENTIFIER
        {
            internal IntPtr pszObjId;
            internal CRYPT_OBJID_BLOB Parameters;
        }

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct CRYPT_BIT_BLOB
        {
            internal UInt32 cbData;
            internal IntPtr pbData;
            internal UInt32 cUnusedBits;
        }

        /// <summary>Structure from Crypto API.</summary>
        [StructLayout(LayoutKind.Sequential)]
        struct CERT_PUBLIC_KEY_INFO
        {
            internal CRYPT_ALGORITHM_IDENTIFIER Algorithm;
            internal CRYPT_BIT_BLOB PublicKey;
        }

        #endregion P/Invoke Structures

        #region P/Invoke Functions

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDestroyKey(IntPtr hKey);

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptImportKey(IntPtr hProv, byte[] pbKeyData, UInt32 dwDataLen, IntPtr hPubKey, UInt32 dwFlags, ref IntPtr hKey);

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

        /// <summary>Function for Crypto API.</summary>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, CRYPT_PROVIDER_TYPE dwProvType, CRYPT_ACQUIRE_CONTEXT_FLAGS dwFlags);

        /// <summary>Function from Crypto API.</summary>
        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptStringToBinary(string sPEM, UInt32 sPEMLength, CRYPT_STRING_FLAGS dwFlags, [Out] byte[] pbBinary, ref UInt32 pcbBinary, out UInt32 pdwSkip, out UInt32 pdwFlags);

        /// <summary>Function from Crypto API.</summary>
        [DllImport("crypt32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecodeObjectEx(CRYPT_ENCODING_FLAGS dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, CRYPT_DECODE_FLAGS dwFlags, IntPtr pDecodePara, ref byte[] pvStructInfo, ref UInt32 pcbStructInfo);

        /// <summary>Function from Crypto API.</summary>
        [DllImport("crypt32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecodeObject(CRYPT_ENCODING_FLAGS dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, CRYPT_DECODE_FLAGS flags, [In, Out] byte[] pvStructInfo, ref UInt32 cbStructInfo);

        #endregion P/Invoke Functions
    }

    #endregion


    #region simpler aes

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
    #endregion

}
