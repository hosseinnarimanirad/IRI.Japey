using DocumentFormat.OpenXml.Bibliography;
using IRI.Extensions;
using IRI.Msh.Common.Ogc;
using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Primitives.SqlServerNativeBinary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IRI.Msh.Common.Helpers;

public static class SqlServerSpatialNativeBinary
{
    private static SqlServerSpatialNativeBinaryTypes ParseType(GeometryType type)
    {
        return type switch
        {
            GeometryType.Point => SqlServerSpatialNativeBinaryTypes.Point,
            //GeometryType.PointZ => SqlServerSpatialNativeBinaryTypes.PointZ,
            //GeometryType.PointM => SqlServerSpatialNativeBinaryTypes.PointM,
            //GeometryType.PointZM => SqlServerSpatialNativeBinaryTypes.PointZM,
            GeometryType.MultiPoint => SqlServerSpatialNativeBinaryTypes.MultiPoint,
            //GeometryType.MultiPointZ => SqlServerSpatialNativeBinaryTypes.MultiPointZ,
            //GeometryType.MultiPointM => SqlServerSpatialNativeBinaryTypes.MultiPointM,
            //GeometryType.MultiPointZM => SqlServerSpatialNativeBinaryTypes.MultiPointZM,



            _ => throw new NotImplementedException($"Geometry type {type} is not implemented.")
        };
    }

    #region SQL Server Native Binary To Geometry

    public static Geometry<Point> Deserialize(byte[] nativeBinary)
    {
        if (nativeBinary.IsNullOrEmpty())
            return null;

        using (var stream = new BinaryReader(new MemoryStream(nativeBinary)))
        {
            var srid = stream.ReadInt32();

            var version = stream.ReadByte();

            var type = (SqlServerSpatialNativeBinaryTypes)stream.ReadByte();

            switch (type)
            {
                case SqlServerSpatialNativeBinaryTypes.Point:
                    return ParsePoint(stream, srid);

                case SqlServerSpatialNativeBinaryTypes.PointZ:
                    return ParsePointZ(stream, srid);

                case SqlServerSpatialNativeBinaryTypes.PointM:
                    return ParsePointM(stream, srid);

                case SqlServerSpatialNativeBinaryTypes.PointZM:
                    return ParsePointZM(stream, srid);

                case SqlServerSpatialNativeBinaryTypes.MultiPoint:
                    return ParseMultiPoint(stream, srid);

                case SqlServerSpatialNativeBinaryTypes.MultiPointZ:
                    break;
                case SqlServerSpatialNativeBinaryTypes.MultiPointM:
                    break;
                case SqlServerSpatialNativeBinaryTypes.MultiPointZM:
                    break;
                default:
                    break;
            }

            //switch (type)
            //{
            //    case WkbGeometryType.Point:
            //        return FromWkbPoint(stream);

            //    case WkbGeometryType.LineString:
            //        return FromWkbLineString(stream);

            //    case WkbGeometryType.Polygon:
            //        return FromWkbPolygon(stream);

            //    case WkbGeometryType.MultiPoint:
            //        return FromWkbMultiPoint(stream);

            //    case WkbGeometryType.MultiLineString:
            //        return FromWkbMultiLineString(stream);

            //    case WkbGeometryType.MultiPolygon:
            //        return FromWkbMultiPolygon(stream);

            //    case WkbGeometryType.PointZ:
            //    case WkbGeometryType.LineStringZ:
            //    case WkbGeometryType.PolygonZ:
            //    case WkbGeometryType.MultiPointZ:
            //    case WkbGeometryType.MultiLineStringZ:
            //    case WkbGeometryType.MultiPolygonZ:

            //    case WkbGeometryType.PointM:
            //    case WkbGeometryType.LineStringM:
            //    case WkbGeometryType.PolygonM:
            //    case WkbGeometryType.MultiPointM:
            //    case WkbGeometryType.MultiLineStringM:
            //    case WkbGeometryType.MultiPolygonM:

            //    case WkbGeometryType.PointZM:
            //    case WkbGeometryType.LineStringZM:
            //    case WkbGeometryType.PolygonZM:
            //    case WkbGeometryType.MultiPointZM:
            //    case WkbGeometryType.MultiLineStringZM:
            //    case WkbGeometryType.MultiPolygonZM:
            //    default:
            //        throw new NotImplementedException();
            //}
        }

        throw new NotImplementedException();
    }

    private static Geometry<Point> ParsePoint(BinaryReader reader, int srid)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();

        return Geometry<Point>.Create(x, y, srid);
    }

    private static Geometry<Point> ParsePointM(BinaryReader reader, int srid)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var m = reader.ReadDouble();

        return Geometry<Point>.Create(x, y, /*m, */srid);
    }

    private static Geometry<Point> ParsePointZ(BinaryReader reader, int srid)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();

        return Geometry<Point>.Create(x, y, /*z, */srid);
    }

    private static Geometry<Point> ParsePointZM(BinaryReader reader, int srid)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();
        var m = reader.ReadDouble();

        return Geometry<Point>.Create(x, y, /*z, */srid);
    }



    private static Geometry<Point> ParseMultiPoint(BinaryReader reader, int srid)
    {
        var numberOfGeometries = reader.ReadInt32();

        var geometries = new List<Geometry<Point>>(numberOfGeometries);

        for (int i = 0; i < numberOfGeometries; i++)
        {
            geometries.Add(ParsePoint(reader, srid));
        }

        return new Geometry<Point>(geometries, GeometryType.MultiPoint, srid);
    }


    private static Geometry<Point> FromWkbLineString(BinaryReader reader, int srid)
    {
        var points = ReadLineStringOrRing(reader, isRing: false);

        return Geometry<Point>.CreatePointOrLineString(points, srid);
    }

    private static Geometry<Point> FromWkbPolygon(BinaryReader reader, int srid)
    {
        var numberOfRings = reader.ReadInt32();

        var geometries = new List<Geometry<Point>>();

        for (int i = 0; i < numberOfRings; i++)
        {
            var ring = ReadLineStringOrRing(reader, isRing: true);

            geometries.Add(Geometry<Point>.CreatePointOrLineString(ring, srid));
        }

        return Geometry<Point>.CreatePolygonOrMultiPolygon(geometries, srid);
    }

    private static Geometry<Point> FromWkbMultiLineString(BinaryReader reader, int srid)
    {
        var numberOfGeometries = reader.ReadInt32();

        var geometries = new List<Geometry<Point>>(numberOfGeometries);

        for (int i = 0; i < numberOfGeometries; i++)
        {
            var byteOrder = reader.ReadByte();

            var type = reader.ReadInt32();

            geometries.Add(FromWkbLineString(reader, srid));
        }

        return new Geometry<Point>(geometries, GeometryType.MultiLineString, srid);
    }

    private static Geometry<Point> FromWkbMultiPolygon(BinaryReader reader, int srid)
    {
        var numberOfGeometries = reader.ReadInt32();

        var geometries = new List<Geometry<Point>>(numberOfGeometries);

        for (int i = 0; i < numberOfGeometries; i++)
        {
            var byteOrder = reader.ReadByte();

            var type = reader.ReadInt32();

            geometries.Add(FromWkbPolygon(reader, srid));
        }

        return new Geometry<Point>(geometries, GeometryType.MultiPolygon, srid);
    }


    private static List<Point> ReadLineStringOrRing(BinaryReader reader, bool isRing)
    {
        var numberOfPoints = reader.ReadInt32();

        var result = new List<Point>(numberOfPoints);

        for (int i = 0; i < numberOfPoints; i++)
        {
            var x = reader.ReadDouble();

            var y = reader.ReadDouble();

            // last point is repeated in rings
            if (isRing && i == numberOfPoints - 1)
                break;

            result.Add(new Point(x, y));
        }

        return result;
    }

    #endregion



    #region Geometry To SQL Server Native Binary

    public static byte[] Serialize<T>(Geometry<T> geometry) where T : IPoint, new()
    {
        if (geometry.IsNullOrEmpty())
            return null;

        using (var ms = new MemoryStream())
        using (var bw = new BinaryWriter(ms))
        {
            bw.Write(geometry.Srid);          // SRID (little-endian)
            bw.Write((byte)0x01);    // Version marker
            bw.Write((int)ParseType(geometry.Type));    // Version marker

            switch (geometry.Type)
            {
                case GeometryType.Point:
                    bw.Write(geometry.Points[0].AsSqlServerNativeBinary());
                    break;

                case GeometryType.LineString:
                    //GeometryLineStringAsWkb(geometry);

                case GeometryType.Polygon:
                    //return GeometryPolygonAsWkb(geometry);

                case GeometryType.MultiPoint:
                    //GeometryMultiPointAsWkb(geometry);

                case GeometryType.MultiLineString:
                    //GeometryMultiLineStringAsWkb(geometry);

                case GeometryType.MultiPolygon:
                    //GeometryMultiPolygonAsWkb(geometry);

                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }

            return ms.ToArray();
        }
    }




    private static byte[] GeometryLineStringAsWkb<T>(Geometry<T> lineString) where T : IPoint, new()
    {
        List<byte> result = new List<byte>();

        result.AddRange(OgcWkbMapFunctions.ToWkbLineString(lineString.Points));

        return result.ToArray();
    }

    // todo: do not modify input polygon. consider add/remove points as a bad practice!
    private static byte[] GeometryPolygonAsWkb<T>(Geometry<T> polygon) where T : IPoint, new()
    {
        List<byte> result = new List<byte>
        {
            (byte)WkbByteOrder.WkbNdr
        };

        result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.Polygon));

        result.AddRange(BitConverter.GetBytes((uint)polygon.Geometries.Count));

        for (int i = 0; i < polygon.Geometries.Count; i++)
        {
            var points = polygon.Geometries[i].Points;
            // add first point
            points.Add(polygon.Geometries[i].Points[0]);

            result.AddRange(OgcWkbMapFunctions.ToWkbLinearRing(points));

            // to enforce idempotency
            points.RemoveAt(points.Count - 1);
        }

        return result.ToArray();
    }

    private static byte[] GeometryMultiPointAsWkb<T>(Geometry<T> multipoint) where T : IPoint, new()
    {
        List<byte> result =
        [
            (byte)WkbByteOrder.WkbNdr,
            .. BitConverter.GetBytes((uint)WkbGeometryType.MultiPoint),
            .. BitConverter.GetBytes((uint)multipoint.Geometries.Count),
        ];

        for (int i = 0; i < multipoint.Geometries.Count; i++)
        {
            result.AddRange(Serialize(multipoint.Geometries[i]));
        }

        return result.ToArray();
    }

    private static byte[] GeometryMultiLineStringAsWkb<T>(Geometry<T> multiLineString) where T : IPoint, new()
    {
        List<byte> result = new List<byte>
        {
            (byte)WkbByteOrder.WkbNdr
        };

        result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.MultiLineString));

        result.AddRange(BitConverter.GetBytes((uint)multiLineString.Geometries.Count));

        for (int i = 0; i < multiLineString.Geometries.Count; i++)
        {
            result.AddRange(Serialize(multiLineString.Geometries[i]));
        }

        return result.ToArray();
    }

    private static byte[] GeometryMultiPolygonAsWkb<T>(Geometry<T> multiPolygon) where T : IPoint, new()
    {
        List<byte> result = new List<byte>
        {
            (byte)WkbByteOrder.WkbNdr
        };

        result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.MultiPolygon));

        result.AddRange(BitConverter.GetBytes((uint)multiPolygon.Geometries.Count));

        for (int i = 0; i < multiPolygon.Geometries.Count; i++)
        {
            result.AddRange(Serialize(multiPolygon.Geometries[i]));
        }

        return result.ToArray();
    }

    #endregion
}
