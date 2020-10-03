using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class GeodeticSheet : IGeometryAware<Point>
    {
        public int Id { get; set; }

        public Geometry<Point> TheGeometry { get => GeodeticExtent.Transform(MapProjects.GeodeticWgs84ToWebMercator).AsGeometry(SridHelper.WebMercator); set => throw new NotImplementedException(); }

        public BoundingBox GeodeticExtent { get; set; }

        public string SheetName { get; set; }

        public string SubTitle { get { return SheetName?.Contains(" ") == true ? SheetName.Split(' ')?.Last() : string.Empty; } }

        public string Note { get; set; }

        public GeodeticIndexType Type { get; set; }

        public GeodeticSheet(BoundingBox geodeticExtent, GeodeticIndexType type)
        {
            this.GeodeticExtent = geodeticExtent;

            this.Type = type;
        }
    }
}
