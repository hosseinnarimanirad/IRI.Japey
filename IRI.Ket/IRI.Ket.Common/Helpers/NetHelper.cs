using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class NetHelper
    {
        const string _defaulHost = "www.google.com";

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

        public static async Task<bool> IsConnectedToInternet(string address = _defaulHost)
        {
            return await PingHostAsync(address);
        }

        public static async Task<bool> IsConnectedToInternet(WebProxy proxy, string address = _defaulHost)
        {
            if (proxy == null)
            {
                return await IsConnectedToInternet(address);
            }
            else
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.Proxy = proxy;

                        using (var stream = await client.OpenReadTaskAsync(address))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
