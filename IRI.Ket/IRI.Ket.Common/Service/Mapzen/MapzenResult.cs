using IRI.Ket.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Mapzen
{

    public class MapzenResult
    {
        public Feature[] features { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Feature
    {
        public Properties properties { get; set; }
        public string type { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Properties
    {
        public float fillOpacity { get; set; }
        public string color { get; set; }
        public string fill { get; set; }
        public string fillColor { get; set; }
        public int contour { get; set; }
        public float opacity { get; set; }
        public float fillopacity { get; set; }
    }

    public class Geometry
    {
        public object coordinates { get; set; }
        public string type { get; set; }
        public Ham.SpatialBase.Primitives.GeometryType Type
        {
            get
            {
                if (type.ToLower().Trim() == "linestring")
                {
                    return Ham.SpatialBase.Primitives.GeometryType.LineString;
                }
                else if (type.ToLower().Trim() == "polygon")
                {
                    return Ham.SpatialBase.Primitives.GeometryType.Polygon;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public double[][] Points
        {
            get
            {
                if (Type == Ham.SpatialBase.Primitives.GeometryType.LineString)
                {
                    return JsonConvert.DeserializeObject<double[][]>(coordinates.ToString());
                }

                return null;
            }
        }

        public  double[][][] Rings
        {
            get
            {
                if (Type == Ham.SpatialBase.Primitives.GeometryType.Polygon)
                {
                    return JsonConvert.DeserializeObject<double[][][]>(coordinates.ToString());
                }

                return null;
            }
        }

        public IRI.Ham.SpatialBase.Primitives.Geometry ParseToGeometry()
        {
            if (type.ToLower().Trim() == "linestring")
            {
                return GeometryHelper.ParseLineStringToGeometry(Points, Ham.SpatialBase.Primitives.GeometryType.LineString, true);
            }
            else if (type.ToLower().Trim() == "polygon")
            {
                return GeometryHelper.ParsePolygonToGeometry(Rings, Ham.SpatialBase.Primitives.GeometryType.Polygon, true);
            }
            else
            {
                throw new NotImplementedException();
            }

        }
    }

}
