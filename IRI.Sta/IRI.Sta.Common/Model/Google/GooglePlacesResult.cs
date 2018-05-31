using IRI.Sta.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Model.Google
{
    public class GoogleLocation
    {

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }

    //public class Northeast
    //{

    //    [JsonProperty("lat")]
    //    public double Lat { get; set; }

    //    [JsonProperty("lng")]
    //    public double Lng { get; set; }
    //}

    //public class Southwest
    //{

    //    [JsonProperty("lat")]
    //    public double Lat { get; set; }

    //    [JsonProperty("lng")]
    //    public double Lng { get; set; }
    //}

    public class Viewport
    {

        [JsonProperty("northeast")]
        public GoogleLocation Northeast { get; set; }

        [JsonProperty("southwest")]
        public GoogleLocation Southwest { get; set; }
    }

    public class Geometry
    {

        [JsonProperty("location")]
        public GoogleLocation Location { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public class Photo
    {

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("html_attributions")]
        public string[] HtmlAttributions { get; set; }

        [JsonProperty("photo_reference")]
        public string PhotoReference { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }

    public class OpeningHours
    {

        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }

        [JsonProperty("weekday_text")]
        public object[] WeekdayText { get; set; }
    }

    public class GooglePlaces
    {

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }

        [JsonProperty("photos")]
        public Photo[] Photos { get; set; }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        public Point AsGeodeticPoint()
        {
            if (this.Geometry?.Location == null)
            {
                return Point.NaN;
            }

            return new Point(this.Geometry.Location.Longitude, this.Geometry.Location.Latitude);
        }

        public Point AsWebMercatorPoint()
        {
            return IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(AsGeodeticPoint());
        }

        public BoundingBox AsGeodeticBoundingBox()
        {
            if (this.Geometry?.Viewport == null)
            {
                return BoundingBox.NaN;
            }

            return new BoundingBox(xMin: this.Geometry.Viewport.Southwest.Longitude,
                yMin: this.Geometry.Viewport.Southwest.Latitude,
                xMax: this.Geometry.Viewport.Northeast.Longitude,
                yMax: this.Geometry.Viewport.Northeast.Latitude);
        }

        public BoundingBox AsWebMercatorBoundingBox()
        {
            return (AsGeodeticBoundingBox().Transform(IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator));
        }
    }

    public class GooglePlacesResult
    {

        [JsonProperty("html_attributions")]
        public object[] HtmlAttributions { get; set; }

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }

        [JsonProperty("results")]
        public GooglePlaces[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
