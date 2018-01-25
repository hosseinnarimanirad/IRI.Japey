using IRI.Ham.Common.Model.Here;
using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Here
{
    public static class HereRoutingApi
    {
        public static async Task<HereIsolineResult> GetIsoline(Point centerGeographic, double timeLimit, string appCode, string appId)
        {
            try
            {
                var pointString = $"{centerGeographic.Y.ToString(CultureInfo.InvariantCulture)},{centerGeographic.X.ToString(CultureInfo.InvariantCulture)}";

                var url = $"https://isoline.route.api.here.com/routing/7.2/calculateisoline.json?app_id={appId}&app_code={appCode}&start=geo!{pointString}&range={timeLimit}&rangetype=time&mode=shortest;car;traffic:enabled";

                return await Helpers.NetHelper.HttpGet<HereIsolineResult>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
