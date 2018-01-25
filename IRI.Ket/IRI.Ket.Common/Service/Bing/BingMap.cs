using IRI.Ham.Common.Model.Bing;
using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Bing
{
    public static class BingMap
    {
        public static async Task<BingIsolineResult> GetIsoline(Point centerGeographic, double timeLimit, string key)
        {
            try
            {
                var pointString = $"{centerGeographic.Y.ToString(CultureInfo.InvariantCulture)},{centerGeographic.X.ToString(CultureInfo.InvariantCulture)}";

                var url = $"https://dev.virtualearth.net/REST/v1/Routes/Isochrones?waypoint={pointString}&maxTime={timeLimit}&timeUnit=second&key={key}";

                return await Helpers.NetHelper.HttpGet<BingIsolineResult>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
