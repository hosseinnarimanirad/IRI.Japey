using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public abstract class IndexBase : IGeometryAware
    {
        [JsonProperty("shne")]
        public string SheetNameEn { get; set; }

        [JsonProperty("shnf")]
        public string SheetNameFa { get; set; }

        [JsonProperty("shno")]
        public string SheetNumber { get; set; }

        [JsonProperty("minlat")]
        public double MinLatitude { get; set; }

        [JsonProperty("minlng")]
        public double MinLongitude { get; set; }

        [JsonIgnore]
        public abstract double Width { get; }

        [JsonIgnore]
        public abstract double Height { get; }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(MinLongitude, MinLatitude, MinLongitude + Width, MinLatitude + Height);
        }

        [JsonIgnore]
        public Geometry Geometry
        {
            //1397.03.21
            get => GetBoundingBox().Transform(MapProjects.GeodeticWgs84ToWebMercator).AsGeometry(SridHelper.GeodeticWGS84);
            set => throw new NotImplementedException();
        }

    }
}
