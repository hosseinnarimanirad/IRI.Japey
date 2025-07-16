using IRI.Sta.Common.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json; 

namespace IRI.Sta.Common.Helpers;

public static class HttpClientHelper
{
    public static async Task<Response<T>> HttpPostAsync<T>(
        HttpClient client,
        HttpParameters parameters) where T : class
    {
        try
        {
            var json = JsonHelper.SerializeWithIgnoreNullOption(parameters.Data);

            var content = new StringContent(json, parameters.Encoding);

            var result = await client.PostAsync(parameters.Address, content);

            result.EnsureSuccessStatusCode();

            var jsonString = await result.Content.ReadAsStringAsync();

            return ResponseFactory.Create(JsonHelper.Deserialize<T>(jsonString));
        }
        catch (Exception ex)
        {
            return ResponseFactory.CreateError<T>(ex.Message);
        }
    }

}
