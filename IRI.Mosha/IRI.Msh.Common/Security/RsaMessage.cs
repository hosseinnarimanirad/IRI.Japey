using IRI.Ket.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Security
{
    public class RsaMessage
    {
        //this is in base64 format
        [JsonProperty("value")]
        public string Message { get; set; }

        public RsaMessage()
        {

        }


        public static RsaMessage Create(object value, string base64PubKey)
        {
            try
            {
                RsaMessage result = new RsaMessage();

                var message = JsonConvert.SerializeObject(value);

                result.Message = CryptographyHelper.RsaEncrypt(message, base64PubKey);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public T Decrypt<T>(string base64PriKey) where T : class
        {
            try
            {
                var decryptedMessage = CryptographyHelper.RsaDecrypt(Message, base64PriKey);
                  
                return JsonConvert.DeserializeObject<T>(decryptedMessage);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
