using IRI.Extensions;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Spatial.IO;
using IRI.Sta.Common.Primitives;

namespace IRI.Sta.Spatial.IO;

public static partial class SqlServerSpatialNativeBinary
{


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
                    return ParseMultiPointZ(stream, srid);

                case SqlServerSpatialNativeBinaryTypes.MultiPointM:
                    return ParseMultiPointM(stream, srid);
                    
                case SqlServerSpatialNativeBinaryTypes.MultiPointZM:
                    return ParseMultiPointZM(stream, srid);
                    
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



    private static Geometry<Point> ParseMultiPoint(BinaryReader reader, int srid, Func<BinaryReader, int, Geometry<Point>> parsePoint)
    {
        var numberOfGeometries = reader.ReadInt32();

        var geometries = new List<Geometry<Point>>(numberOfGeometries);

        for (int i = 0; i < numberOfGeometries; i++)
        {
            geometries.Add(parsePoint(reader, srid));
        }

        return new Geometry<Point>(geometries, GeometryType.MultiPoint, srid);
    }


    private static Geometry<Point> ParseMultiPoint(BinaryReader reader, int srid) => ParseMultiPoint(reader, srid, ParsePoint);

    private static Geometry<Point> ParseMultiPointM(BinaryReader reader, int srid) => ParseMultiPoint(reader, srid, ParsePointM);
    private static Geometry<Point> ParseMultiPointZ(BinaryReader reader, int srid) => ParseMultiPoint(reader, srid, ParsePointZ);
    private static Geometry<Point> ParseMultiPointZM(BinaryReader reader, int srid) => ParseMultiPoint(reader, srid, ParsePointZM);


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



}
