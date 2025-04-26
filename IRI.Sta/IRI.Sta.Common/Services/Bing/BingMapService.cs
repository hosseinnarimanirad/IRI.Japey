using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Contracts.Bing;
using IRI.Extensions;
using System;
using System.Globalization;
using System.Threading.Tasks; 

namespace IRI.Sta.Common.Services.Bing;

public static class BingMapService
{
    public static async Task<Response<BingIsolineResult>> GetIsolineAsync(Point centerGeographic, double timeLimit, string key)
    {
        try
        {
            var pointString = $"{centerGeographic.Y.ToString(CultureInfo.InvariantCulture)},{centerGeographic.X.ToString(CultureInfo.InvariantCulture)}";

            var url = $"https://dev.virtualearth.net/REST/v1/Routes/Isochrones?waypoint={pointString}&maxTime={timeLimit}&timeUnit=second&key={key}";

            return await Helpers.NetHelper.HttpGetAsync<BingIsolineResult>(url);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            return ResponseFactory.CreateError<BingIsolineResult>(ex.GetMessagePlus());
        }
    }
}