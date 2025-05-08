﻿using System;
using System.Text.Json;

namespace IRI.Sta.Common.Contracts.Mapzen;

public class MapzenIsochroneResult
{
    public MapzenFeature[] features { get; set; }
    public string id { get; set; }
    public string type { get; set; }
}

public class MapzenFeature
{
    public MapzenProperties properties { get; set; }
    public string type { get; set; }
    public MapzenGeometry geometry { get; set; }
}

public class MapzenProperties
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
    public IRI.Sta.Common.Primitives.GeometryType Type
    {
        get
        {
            if (type.ToLower().Trim() == "linestring")
            {
                return IRI.Sta.Common.Primitives.GeometryType.LineString;
            }
            else if (type.ToLower().Trim() == "polygon")
            {
                return IRI.Sta.Common.Primitives.GeometryType.Polygon;
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
            if (Type == IRI.Sta.Common.Primitives.GeometryType.LineString)
            {
                return JsonSerializer.Deserialize<double[][]>(coordinates.ToString());
            }

            return null;
        }
    }

    public double[][][] Rings
    {
        get
        {
            if (Type == IRI.Sta.Common.Primitives.GeometryType.Polygon)
            {
                return JsonSerializer.Deserialize<double[][][]>(coordinates.ToString());
            }

            return null;
        }
    }

    //public IRI.Sta.Common.Primitives.Geometry<Point> ParseToGeometry()
    //{
    //    if (type.ToLower().Trim() == "linestring")
    //    {
    //        return IRI.Sta.Common.Primitives.Geometry<Point>.ParseLineStringToGeometry(Points, IRI.Sta.Common.Primitives.GeometryType.LineString, true);
    //    }
    //    else if (type.ToLower().Trim() == "polygon")
    //    {
    //        return IRI.Sta.Common.Primitives.Geometry<Point>.ParsePolygonToGeometry(Rings, IRI.Sta.Common.Primitives.GeometryType.Polygon, true);
    //    }
    //    else
    //    {
    //        throw new NotImplementedException();
    //    }

    //}
}
