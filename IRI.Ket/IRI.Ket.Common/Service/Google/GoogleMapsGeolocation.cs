using IRI.Ket.Common.Devices.ManagedNativeWifi;
using IRI.Ket.Common.Service.Google.ApiParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleMapsGeolocation
    {
        public static async Task<ApiResults.GoogleMapsGeolocationJsonResult> GetLocation(string key)
        {
            try
            {
                var networks = NativeWifi.EnumerateBssNetworks();

                var parameter = new GoogleMapsGeolocationParameters()
                {
                    wifiAccessPoints = networks.Select(i => new Wifiaccesspoint()
                    {
                        channel = i.Channel,
                        macAddress = i.Bssid.ToString(),
                        signalStrength = i.SignalStrength,

                    }).ToArray()
                };

                var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";

                return await Helpers.NetHelper.HttpPost<ApiResults.GoogleMapsGeolocationJsonResult>(url, parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return new ApiResults.GoogleMapsGeolocationJsonResult() { accuracy = int.MaxValue };
            }
        }
    }
}

