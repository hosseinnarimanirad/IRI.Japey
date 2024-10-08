﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Service.Oxford
{
    public static class OxfordDictionaryService
    {
        const string baseUrl = "https://od-api.oxforddictionaries.com/api/v1/";
         
        public static async Task<OdResult> GetExamples(string word, string appId, string appKey)
        {
            try
            {
                using (WebClient client = new WebClient())
                {

                    client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                    client.Headers.Add("app_id", appId);
                    client.Headers.Add("app_key", appKey);
                    client.Encoding = Encoding.UTF8;

                    var result = await client.DownloadStringTaskAsync($"https://od-api.oxforddictionaries.com/api/v1/entries/en/{word.ToLower()}");

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return null;
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<OdResult>(result);
                    }
                }
            }
            catch (WebException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
