using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Primitives;
using IRI.Sta.CoordinateSystems.MapProjection;

namespace IRI.Ket.GdiPlus.Model;

public class GeoTaggedImage
{
    public Point3D GeographicLocation { get; set; }

    public Point3D WebMercatorLocation { get; set; }

    public string ImageFileName { get; set; }

    public GeoTaggedImage(string imageFileName)
    {
        try
        {
            this.ImageFileName = imageFileName;

            var location = System.IO.Path.ChangeExtension(imageFileName, ".corx");

            if (System.IO.File.Exists(location))
            {
                this.GeographicLocation = JsonHelper.Deserialize<Point3D>(System.IO.File.ReadAllText(location));

                var webMercator = MapProjects.GeodeticWgs84ToWebMercator((Point)GeographicLocation);

                this.WebMercatorLocation = new Point3D(webMercator.X, webMercator.Y, GeographicLocation.Z);
            }
            else
            {
                using (var bitmap = new System.Drawing.Bitmap(imageFileName))
                {
                    this.GeographicLocation = Helpers.ImageHelper.GetWgs84Location(bitmap);

                    var webMercator = MapProjects.GeodeticWgs84ToWebMercator((Point)GeographicLocation);

                    this.WebMercatorLocation = new Point3D(webMercator.X, webMercator.Y, GeographicLocation.Z);

                    System.IO.File.WriteAllText(location, JsonHelper.Serialize(this.GeographicLocation));
                }
            }
        }
        catch (Exception)
        {
            this.GeographicLocation = Point3D.NaN;

            this.WebMercatorLocation = Point3D.NaN;
        }
    }
}
