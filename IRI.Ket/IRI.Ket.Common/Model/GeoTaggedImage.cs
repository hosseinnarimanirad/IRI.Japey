using IRI.Ham.CoordinateSystem;
using IRI.Ham.CoordinateSystem.MapProjection;
using IRI.Ham.SpatialBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model
{
    public class GeoTaggedImage
    {
        public Point3D GeographicLocation { get; set; }

        public Point3D MercatorLocation { get; set; }

        public string ImageFileName { get; set; }

        public GeoTaggedImage(string imageFileName)
        {
            try
            {
                this.ImageFileName = imageFileName;

                var location = System.IO.Path.ChangeExtension(imageFileName, ".corx");

                if (System.IO.File.Exists(location))
                {
                    this.GeographicLocation = JsonConvert.DeserializeObject<Point3D>(System.IO.File.ReadAllText(location));

                    var webMercator = MapProjects.GeodeticWgs84ToWebMercator((Point)GeographicLocation);

                    this.MercatorLocation = new Point3D(webMercator.X, webMercator.Y, GeographicLocation.Z);
                }
                else
                {
                    using (var bitmap = new System.Drawing.Bitmap(imageFileName))
                    {
                        this.GeographicLocation = Helpers.ImageHelper.GetWgs84Location(bitmap);

                        var webMercator = MapProjects.GeodeticWgs84ToWebMercator((Point)GeographicLocation);

                        this.MercatorLocation = new Point3D(webMercator.X, webMercator.Y, GeographicLocation.Z);

                        System.IO.File.WriteAllText(location, JsonConvert.SerializeObject(this.GeographicLocation));
                    }
                }
            }
            catch (Exception)
            {
                this.GeographicLocation = Point3D.NaN;

                this.MercatorLocation = Point3D.NaN;
            }
        }
    }
}
