using IRI.Ham.SpatialBase;
using IRI.Ham.SpatialBase.Model.Google;
using System;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleDirectionsService
    {
        public static async Task<Response<GoogleDirectionsResult>> GetDirectionAsync(Point start, Point end, string key)
        {
            try
            {
                var startString = GoogleServiceUtility.ParseToString(start);

                var endString = GoogleServiceUtility.ParseToString(end);

                var time = GoogleServiceUtility.GetNow();

                var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={startString}&destination={endString}&alternatives=true&departure_time={time}&key={key}";

                return await Helpers.NetHelper.HttpGetAsync<GoogleDirectionsResult>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return ResponseFactory.CreateError<GoogleDirectionsResult>(ex.Message);
            }
        }
    }
}
