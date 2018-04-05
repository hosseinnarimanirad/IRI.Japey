using IRI.Ham.SpatialBase.Model.Google;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GooglePlaces
    {
        const string key = "AIzaSyCjCpF7qLLBRs1JjBVlfhCI3fJnN3Ovj-Q";

        const string apiUrl = "https://maps.googleapis.com/maps/api/place/textsearch/json?";

        static WebClient client = new WebClient();

        static GooglePlaces()
        {
            client.Headers.Add("user-agent", "App!");

            client.Encoding = Encoding.UTF8;
        }

        public async static Task<GooglePlacesResult> Search(string query)
        {
            var result = await client.DownloadStringTaskAsync(new Uri($"{apiUrl}query={query}&key={key}"));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<GooglePlacesResult>(result);
        }
    }
}
