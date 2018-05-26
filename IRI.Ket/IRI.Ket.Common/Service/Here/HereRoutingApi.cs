using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Model.Here;
using IRI.Ket.Common.Extensions;
using System;
using System.Globalization;
using System.Threading.Tasks;
using IRI.Sta.Common.Primitives;

namespace IRI.Ket.Common.Service.Here
{
    public static class HereRoutingApi
    {
        public static async Task<Response<HereIsolineResult>> GetIsolineAsync(Point centerGeographic, double timeLimit, string appCode, string appId)
        {
            try
            {
                var pointString = $"{centerGeographic.Y.ToString(CultureInfo.InvariantCulture)},{centerGeographic.X.ToString(CultureInfo.InvariantCulture)}";

                var url = $"https://isoline.route.api.here.com/routing/7.2/calculateisoline.json?app_id={appId}&app_code={appCode}&start=geo!{pointString}&range={timeLimit}&rangetype=time&mode=shortest;car;traffic:enabled";

                return await Helpers.NetHelper.HttpGetAsync<HereIsolineResult>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return ResponseFactory.CreateError<HereIsolineResult>(ex.GetMessagePlus());
            }
        }
    }
}
