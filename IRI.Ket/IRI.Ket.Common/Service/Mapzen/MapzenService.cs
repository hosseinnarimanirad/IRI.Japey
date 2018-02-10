using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Mapzen
{
    public static class MapzenService
    {
        public static async Task<MapzenResult> GetIsochrone(Point geodeticPoint, string apiKey)
        {
          //  var url = $"matrix.mapzen.com/isochrone?json={{\"locations\":[{{\"lat\":{geodeticPoint.Y},\"lon\":{geodeticPoint.X}}}],\"costing\":\"bicycle\",\"contours\":[{{\"time\":15,\"color\":\"ff0000\"}}]}}&id=Walk_From_Office&api_key={apiKey}";

            var parameter = new MapzenRouteCostingModel()
            {
                locations = new Location[] { new Location() { lon = geodeticPoint.X, lat = geodeticPoint.Y } },
                costing = "auto",
                contours = new Contour[] { new Contour() { time = 4, color = "ff0000" } },
                polygons = true,
            };

            var url = $"http://matrix.mapzen.com/isochrone?id=Walk_From_Office&api_key={apiKey}";

            return await Helpers.NetHelper.HttpPost<MapzenResult>(url, parameter);
        }
    }




}
