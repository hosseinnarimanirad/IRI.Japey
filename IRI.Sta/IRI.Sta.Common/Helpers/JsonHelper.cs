using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Helpers;

public static class JsonHelper
{
    static readonly JsonSerializerOptions _ignoreNullValue;

    static JsonHelper()
    {
        _ignoreNullValue = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public static string Serialize<T>(T value)
    {
        //return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        return System.Text.Json.JsonSerializer.Serialize(value);
    }

    public static string Serialize<T>(T value, bool indented)
    {
        return JsonSerializer.Serialize(
                value,
                new JsonSerializerOptions
                {
                    WriteIndented = indented, // true for pretty-printed, false for compact
                });
    }

    public static string SerializeWithIgnoreNullOption<T>(T value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, _ignoreNullValue);
    }

    public static T Deserialize<T>(string jsonString)
    {
        //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static T Deserialize<T>(string jsonString, System.Text.Json.Serialization.JsonConverter converter)
    {
        //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        return System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions()
        {
            Converters = { converter },
            PropertyNameCaseInsensitive = true
        });
    }

}
