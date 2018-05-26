using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Model.Mapzen
{
    public class MapzenIsochroneResult
    {
        public Feature[] features { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Feature
    {
        public Properties properties { get; set; }
        public string type { get; set; }
        public MapzenGeometry geometry { get; set; }
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

    public class MapzenGeometry
    {
        public object coordinates { get; set; }
        public string type { get; set; }
        public Sta.Common.Primitives.GeometryType Type
        {
            get
            {
                if (type.ToLower().Trim() == "linestring")
                {
                    return Sta.Common.Primitives.GeometryType.LineString;
                }
                else if (type.ToLower().Trim() == "polygon")
                {
                    return Sta.Common.Primitives.GeometryType.Polygon;
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
                if (Type == Sta.Common.Primitives.GeometryType.LineString)
                {
                    return JsonConvert.DeserializeObject<double[][]>(coordinates.ToString());
                }

                return null;
            }
        }

        public double[][][] Rings
        {
            get
            {
                if (Type == Sta.Common.Primitives.GeometryType.Polygon)
                {
                    return JsonConvert.DeserializeObject<double[][][]>(coordinates.ToString());
                }

                return null;
            }
        }

        public IRI.Sta.Common.Primitives.Geometry ParseToGeometry()
        {
            if (type.ToLower().Trim() == "linestring")
            {
                return IRI.Sta.Common.Primitives.Geometry.ParseLineStringToGeometry(Points, Sta.Common.Primitives.GeometryType.LineString, true);
            }
            else if (type.ToLower().Trim() == "polygon")
            {
                return IRI.Sta.Common.Primitives.Geometry.ParsePolygonToGeometry(Rings, Sta.Common.Primitives.GeometryType.Polygon, true);
            }
            else
            {
                throw new NotImplementedException();
            }

        }
    }

}
