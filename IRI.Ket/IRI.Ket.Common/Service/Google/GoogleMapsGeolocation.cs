using IRI.Ham.SpatialBase.Model.Google;
using IRI.Ket.Common.Devices.ManagedNativeWifi; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleMapsGeolocation
    {
        public static async Task<GoogleGeolocationResult> GetLocationAsync(string key)
        {
            try
            {
                var networks = NativeWifi.EnumerateBssNetworks();

                var parameter = new GoogleGeolocationParameters()
                {
                    wifiAccessPoints = networks.Select(i => new Wifiaccesspoint()
                    {
                        channel = i.Channel,
                        macAddress = i.Bssid.ToString(),
                        signalStrength = i.SignalStrength,

                    }).ToArray()
                };

                var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";

                return await Helpers.NetHelper.HttpPostAsync<GoogleGeolocationResult>(url, parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return new GoogleGeolocationResult() { accuracy = int.MaxValue };
            }
        }
         
        public static async Task<GoogleGeolocationResult> GetLocationAsync(string key, List<Wifiaccesspoint> networks)
        {
            try
            {
                var parameter = new GoogleGeolocationParameters()
                {
                    wifiAccessPoints = networks.ToArray()
                };

                var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={key}";

                return await Helpers.NetHelper.HttpPostAsync<GoogleGeolocationResult>(url, parameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return new GoogleGeolocationResult() { accuracy = int.MaxValue };
            }
        }

        public static GoogleGeolocationResult GetLocation(string key, List<Wifiaccesspoint> networks)
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

                return new GoogleGeolocationResult() { accuracy = int.MaxValue };
            }
        }

    }
}

