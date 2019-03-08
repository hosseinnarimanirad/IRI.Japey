using IRI.Ket.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Security
{
    public class EncryptedMessage<T> where T : class
    {
        [JsonProperty("k")]
        public string IV { get; set; }

        [JsonProperty("m")]
        public string Token { get; set; }

        [JsonProperty("f")]
        public string Message { get; set; }

        public EncryptedMessage()
        {

        }

        public EncryptedMessage(T value, string base64Token)
        {
            //base64Token is a public key in base64 string format

            RijndaelManaged rm = new RijndaelManaged();

            var encryptedMessage = CryptographyHelper.AesEncrypt(Newtonsoft.Json.JsonConvert.SerializeObject(value), rm.Key, rm.IV);

            this.Token = CryptographyHelper.RsaEncrypt(Convert.ToBase64String(rm.Key), base64Token);

            this.IV = Convert.ToBase64String(rm.IV);

            this.Message = encryptedMessage;
        }

        public T Decrypt(string base64PriKey)
        {
            byte[] key = Convert.FromBase64String(CryptographyHelper.RsaDecrypt(Token, base64PriKey));

            byte[] iv = Convert.FromBase64String(IV);

            var decryptedParameters = CryptographyHelper.AesDecrypt(Message, key, iv);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(decryptedParameters);

        }
    }
}
