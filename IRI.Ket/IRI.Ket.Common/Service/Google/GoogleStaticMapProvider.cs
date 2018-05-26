using IRI.Sta.Common.Primitives;
using IRI.Ket.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IRI.Sta.Common.Model;
using IRI.Sta.Common.Primitives;

namespace IRI.Ket.Common.Service.Google
{
    public static class GoogleStaticMapProvider
    {
        public static async Task<Response<GeoReferencedImage>> GetAsync(Point centerGeoPoint, int width, int height, BoundingBox mbb, int zoom)
        {
            try
            {
                WebClient client = new WebClient();

                var center = mbb.Center;

                var url = $@"https://maps.googleapis.com/maps/api/staticmap?center={center.Y},{center.X}&zoom={zoom}&size={width}x{height}&maptype=roadmap";

                var byteImage = await client.DownloadDataTaskAsync(url);

                return ResponseFactory.Create(new GeoReferencedImage(byteImage, mbb));

            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateError<GeoReferencedImage>(ex.GetMessagePlus());
            }
        }


    }
}
