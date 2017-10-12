using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public class JsonHelper
    {
        public static string ParseToJson<T>(T value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        public static T ParseFromJson<T>(string jsonString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
