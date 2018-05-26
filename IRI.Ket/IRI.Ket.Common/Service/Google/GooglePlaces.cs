using IRI.Sta.Common.Model.Google;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GooglePlacesService
    {
        const string apiUrl = "https://maps.googleapis.com/maps/api/place/textsearch/json?";

        //static WebClient client = new WebClient();

        //static GooglePlaces()
        //{
        //    client.Headers.Add("user-agent", "App!");

        //    client.Encoding = Encoding.UTF8;
        //}

        public async static Task<Response<GooglePlacesResult>> SearchAsync(string query, string key)
        {
            //var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={startString}&destination={endString}&alternatives=true&departure_time={time}&key={key}";

            //return ResponseFactory.Create(await Helpers.NetHelper.HttpGetAsync<GoogleDirectionsResult>(url));

            return await Helpers.NetHelper.HttpGetAsync<GooglePlacesResult>($"{apiUrl}query={query}&key={key}");

            //var result = await client.DownloadStringTaskAsync(new Uri($"{apiUrl}query={query}&key={key}"));

            //return Newtonsoft.Json.JsonConvert.DeserializeObject<GooglePlacesResult>(result);
        }

        public static Response<GooglePlacesResult> Search(string query, string key)
        {
            return Helpers.NetHelper.HttpGet<GooglePlacesResult>($"{apiUrl}query={query}&key={key}");
        }
    }
}
