﻿
using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Model.Google;
using IRI.Ket.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleDistanceMatrixService
    {
        public static async Task<Response<GoogleDistanceMatrixResult>> GetDistanceMatrixAsync(List<Point> origins, List<Point> destinations, string key)
        {
            try
            {
                var originString = string.Join("|", origins.Select(p => $"{p.Y.ToString(CultureInfo.InvariantCulture)},{p.X.ToString(CultureInfo.InvariantCulture)}"));

                var destinationString = string.Join("|", destinations.Select(p => $"{p.Y.ToString(CultureInfo.InvariantCulture)},{p.X.ToString(CultureInfo.InvariantCulture)}"));

                var time = (int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

                var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={originString}&destinations={destinationString}&departure_time={time}&key={key}";

                return await Helpers.NetHelper.HttpGetAsync<GoogleDistanceMatrixResult>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return ResponseFactory.CreateError<GoogleDistanceMatrixResult>(ex.GetMessagePlus());
                //return new Response<GoogleDistanceMatrixResult>() { Result = new GoogleDistanceMatrixResult() { rows = new Row[0] }, IsFailed = true, ErrorMessage = ex.Message };
            }
        }

    }
}
