using IRI.Sta.Common.Model.Google;
using IRI.Ket.Common.Devices.ManagedNativeWifi;
using IRI.Ket.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleMapsGeolocationService
    {
        public static async Task<Response<GoogleGeolocationResult>> GetLocationAsync(string key)
        {
            try
            {
                var networks = NativeWifi.EnumerateBssNetworks();

                var
                    wifiAccessPoints = networks.Select(i => new Wifiaccesspoint()
                    {
                        channel = i.Channel,
                        macAddress = i.Bssid.ToString(),
                        signalStrength = i.SignalStrength,

                    }).ToArray();

                return await GetLocationAsync(key, wifiAccessPoints);

                //var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";

                //return await Helpers.NetHelper.HttpPostAsync<GoogleGeolocationResult>(url, parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                //return new GoogleGeolocationResult() { accuracy = int.MaxValue };
                return ResponseFactory.CreateError<GoogleGeolocationResult>(ex.GetMessagePlus());
            }
        }

        public static async Task<Response<GoogleGeolocationResult>> GetLocationAsync(string key, Wifiaccesspoint[] networks)
        {
            try
            {
                var parameter = new GoogleGeolocationParameters() { wifiAccessPoints = networks };

                var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";

                return await Helpers.NetHelper.HttpPostAsync<GoogleGeolocationResult>(url, parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return ResponseFactory.CreateError<GoogleGeolocationResult>(ex.GetMessagePlus());
            }
        }

        public static Response<GoogleGeolocationResult> GetLocation(string key, List<Wifiaccesspoint> networks)
        {
            try
            {
                var parameter = new GoogleGeolocationParameters()
                {
                    wifiAccessPoints = networks.ToArray()
                };

                var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";

                return Helpers.NetHelper.HttpPost<GoogleGeolocationResult>(url, parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return ResponseFactory.CreateError<GoogleGeolocationResult>(ex.GetMessagePlus());
            }
        }

    }
}

