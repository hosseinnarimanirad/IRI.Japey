using IRI.Extensions;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.IO;
using IRI.Sta.Spatial.IO.OgcSFA;
using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Enums;
using IRI.Sta.Common.Abstrations;

namespace IRI.Sta.Spatial.IO;

public static partial class SqlServerSpatialNativeBinary
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

    private static bool HasZ(SqlServerSpatialNativeBinaryTypes type)
    {
        return type switch
        {
            SqlServerSpatialNativeBinaryTypes.PointZ => true,
            SqlServerSpatialNativeBinaryTypes.PointZM => true,
            SqlServerSpatialNativeBinaryTypes.MultiPointZ => true,
            SqlServerSpatialNativeBinaryTypes.MultiPointZM => true,
            _ => false
        };
    }

    private static bool HasM(SqlServerSpatialNativeBinaryTypes type)
    {
        return type switch
        {
            SqlServerSpatialNativeBinaryTypes.PointM => true,
            SqlServerSpatialNativeBinaryTypes.PointZM => true,
            SqlServerSpatialNativeBinaryTypes.MultiPointM => true,
            SqlServerSpatialNativeBinaryTypes.MultiPointZM => true,
            _ => false
        };
    }




    public static byte[] Serialize<T>(Geometry<T> geometry) where T : IPoint, new()
    {
        if (geometry.IsNullOrEmpty())
            return null;

        using (var ms = new MemoryStream())
        using (var bw = new BinaryWriter(ms))
        {
            bw.Write(geometry.Srid);          // SRID (little-endian)
            bw.Write((byte)0x01);    // Version marker
            bw.Write((byte)ParseType(geometry.Type));    // Version marker

            switch (geometry.Type)
            {
                case GeometryType.Point:
                    bw.Write(geometry.Points[0].AsByteArray());
                    break;

                case GeometryType.LineString:
                //GeometryLineStringAsWkb(geometry);

                case GeometryType.Polygon:
                //return GeometryPolygonAsWkb(geometry);

                case GeometryType.MultiPoint:
                    GeometryMultiPointAsWkb(bw, geometry);
                    break;

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

    private static void GeometryMultiPointAsWkb<T>(BinaryWriter writer, Geometry<T> multipoint) where T : IPoint, new()
    {
        writer.Write(multipoint.NumberOfGeometries);

        for (int i = 0; i < multipoint.Geometries.Count; i++)
        {
            writer.Write(multipoint.Geometries[i].Points[0].AsByteArray());
        }

        // write sql server's metada
        // sample

        //01000000 01 00000000                                     02000000 FFFFFFFF0000000004000000000000000001
        //02000000 01 00000000 01 01000000                         03000000 FFFFFFFF0000000004000000000000000001 00000000 01000000 01
        //03000000 01 00000000 01 01000000 01 02000000             04000000 FFFFFFFF0000000004000000000000000001 00000000 01000000 01 00000000 02000000 01
        //04000000 01 00000000 01 01000000 01 02000000 01 03000000 05000000 FFFFFFFF0000000004000000000000000001 00000000 01000000 01 00000000 02000000 01 00000000 03000000 01

        writer.Write(multipoint.NumberOfGeometries);

        for (int i = 0; i < multipoint.Geometries.Count; i++)
        {
            writer.Write((byte)0x01);
            writer.Write(i);
        }

        writer.Write(multipoint.NumberOfGeometries + 1);

        writer.Write(HexStringHelper.ToByteArray("0xFFFFFFFF0000000004000000000000000001"));

        for (int i = 1; i < multipoint.NumberOfGeometries; i++)
        {
            writer.Write((int)0);
            writer.Write(BitConverter.GetBytes(i));
            writer.Write((byte)0x01);
        }
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

}
