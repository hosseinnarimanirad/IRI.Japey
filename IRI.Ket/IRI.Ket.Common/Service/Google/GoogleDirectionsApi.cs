using IRI.Ham.Common.Model.Google;
using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleDirectionsApi
    {
        public static async Task<GoogleDirectionsResult> GetDirection(Point start, Point end, string key)
        {
            try
            {
                var startString = GoogleServiceUtility.ParseToString(start);

                var endString = GoogleServiceUtility.ParseToString(end);

                var time = GoogleServiceUtility.GetNow();

                var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={startString}&destination={endString}&alternatives=true&departure_time={time}&key={key}";

                return await Helpers.NetHelper.HttpGet<GoogleDirectionsResult>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
