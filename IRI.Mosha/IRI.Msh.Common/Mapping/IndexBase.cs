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
        public string SheetNameEn { get; set; }

        public string SheetNameFa { get; set; }

        public string SheetNumber { get; set; }

        public double MinLatitude { get; set; }

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
            get => GetBoundingBox().Transform(MapProjects.GeodeticWgs84ToWebMercator).AsGeometry(SridHelper.GeodeticWGS84);
            set => throw new NotImplementedException();
        }

    }
}
