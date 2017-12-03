using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class NetHelper
    {
        const string _defaulHost = "www.google.com";

        const string _defaultUri = "https://google.com";

        public static bool PingHost(string nameOrAddress, int timeout = 3000)
        {
            bool pingable = false;

            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(nameOrAddress, timeout);

                pingable = reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                // Discard PingExceptions and return false;
                return false;
            }

            return pingable;
        }

        public static async Task<bool> PingHostAsync(string nameOrAddress, int timeout = 5000)
        {
            bool pingable = false;

            Ping pingSender = new Ping();

            try
            {
                var reply = await pingSender.SendPingAsync(nameOrAddress, timeout);

                pingable = reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                return false;
            }

            return pingable;
        }

        public static async Task<bool> IsConnectedToInternet(WebProxy proxy, string address = null)
        {
            if (proxy == null)
            {
                var isConnected = await PingHostAsync(address ?? _defaulHost);

                if (!isConnected)
                {
                    return await OpenRead(proxy, address ?? _defaultUri);
                }

                return isConnected;
                //return await OpenRead(proxy, address ?? _defaultUri);
            }
            else
            {
                return await OpenRead(proxy, address ?? _defaultUri);
            }
        }

        private static async Task<bool> OpenRead(WebProxy proxy, string address)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, "app!");

                    if (proxy?.Address != null)
                    {
                        client.Proxy = proxy;
                    }

                    using (var stream = await client.OpenReadTaskAsync(new Uri(address, UriKind.Absolute)))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<T> HttpPost<T>(string address, object data) where T : class
        {
            try
            {
                WebClient client = new WebClient();

                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.UserAgent, "application!");

                var stringData = Newtonsoft.Json.JsonConvert.SerializeObject(data, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                });

                var stringResult = await client.UploadStringTaskAsync(address, stringData);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<T> HttpGet<T>(string address) where T : class
        {
            try
            {
                WebClient client = new WebClient();

                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.UserAgent, "application!");
                 
                var stringResult = await client.DownloadStringTaskAsync(address);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
