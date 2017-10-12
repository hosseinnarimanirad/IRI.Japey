using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.GoogleStaticMapProvider
{
    public static class GoogleStaticMapProvider
    {
        public static async Task<GeoReferencedImage> Get(Point centerGeoPoint, int width, int height, BoundingBox mbb, int zoom)
        {
            WebClient client = new WebClient();

            var center = mbb.Center;

            var url = $@"https://maps.googleapis.com/maps/api/staticmap?center={center.Y},{center.X}&zoom={zoom}&size={width}x{height}&maptype=roadmap";

            var byteImage = await client.DownloadDataTaskAsync(url);

            return new GeoReferencedImage(byteImage, mbb);
        }


    }
}
