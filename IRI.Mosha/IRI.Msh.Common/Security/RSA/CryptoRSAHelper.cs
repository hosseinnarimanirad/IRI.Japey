using System;
using System.Security.Cryptography;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Security
{

    /// <summary>
    /// AsymmetricCryptography > RSA
    /// </summary>
    public static class CryptoRSAHelper
    {
        private static byte[] RSA_OID = { 0x30, 0xD, 0x6, 0x9, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0xD, 0x1, 0x1, 0x1, 0x5, 0x0 }; // Object ID for RSA

        // Corresponding ASN identification bytes
        const byte INTEGER = 0x2;
        const byte SEQUENCE = 0x30;
        const byte BIT_STRING = 0x3;
        const byte OCTET_STRING = 0x4;

        //#region JSON
        //internal static void FromJsonString(this RSA rsa, string jsonString)
        //{
        //    try
        //    {
        //        var paramsJson = JsonConvert.DeserializeObject<RSAParametersJson>(jsonString);

        //        RSAParameters parameters = new RSAParameters();

        //        parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
        //        parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
        //        parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
        //        parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
        //        parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
        //        parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
        //        parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
        //        parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
        //        rsa.ImportParameters(parameters);
        //    }
        //    catch
        //    {
        //        throw new Exception("Invalid JSON RSA key.");
        //    }
        //}

        //internal static string ToJsonString(this RSA rsa, bool includePrivateParameters)
        //{
        //    RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        //    var parasJson = new RSAParametersJson()
        //    {
        //        Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
        //        Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
        //        P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
        //        Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
        //        DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
        //        DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
        //        InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
        //        D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
        //    };

        //    return JsonConvert.SerializeObject(parasJson);
        //}
        //#endregion


        #region Encrypt / Decrypt

        /// <summary>
        /// Returns encrypted value in base64string
        /// </summary>
        /// <param name="original"></param>
        /// <param name="base46PublicKey"></param>
        /// <returns>encrypted value in base64string</returns>
        public static string RsaEncrypt(byte[] original, string base46PublicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    Security.CryptoRSAHelper.FromXmlString(rsa, base46PublicKey.Base64ToNormalString());
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simpleString"></param>
        /// <param name="base46PublicKey"></param>
        /// <returns>encrypted value in base64string</returns>
        public static string RsaEncrypt(string simpleString, string base46PublicKey)
        {
            var originalByte = Encoding.UTF8.GetBytes(simpleString);

            return RsaEncrypt(originalByte, base46PublicKey);
        }


        public static string RsaDecrypt(byte[] encoded, string base64PrivateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    Security.CryptoRSAHelper.FromXmlString(rsa, base64PrivateKey.Base64ToNormalString());
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


        #region XML Representation of the Public/Private Keys

        // 1400.02.07
        // initialize RSA with a private or public key in xml format
        public static void FromXmlString(RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }


        // 1400.02.07
        // get the xml format of the RSA private or public key
        public static string ToXmlString(RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }


        public static RsaKeys GenerateKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    return new RsaKeys(ToXmlString(rsa, true), ToXmlString(rsa, false));
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

        #endregion


        #region Binary Representation of the Public/Private Keys 

        public static string GetPublicKeyInBinary(RSAParameters param)
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

        public static string GetPrivateKeyInBinary(RSAParameters param)
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

        public static string GetPublicKeyInBinary(string base64PrivateKey)
        {
            return ConvertKey(base64PrivateKey, false);
        }

        public static string GetPrivateKeyInBinary(string base64PrivateKey)
        {
            return ConvertKey(base64PrivateKey, true);
        }

        private static string ConvertKey(string base64PrivateKey, bool includePrivateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    rsa.FromXmlString(base64PrivateKey.Base64ToNormalString());

                    if (includePrivateKey)
                    {
                        return GetPrivateKeyInBinary(rsa.ExportParameters(includePrivateKey));
                    }
                    else
                    {
                        return GetPublicKeyInBinary(rsa.ExportParameters(includePrivateKey));
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

        #endregion


        #region Private Methods

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

        #endregion
    }
}
