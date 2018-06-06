using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ket.ShapefileFormat.EsriType;

namespace IRI.Ket.ShapefileFormat
{
    public static class OgcWkbMapFunctions
    {
        public static byte[] ToWkbPoint(EsriPoint point)
        {
            byte[] result = new byte[21];

            result[0] = (byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr;

            Array.Copy(BitConverter.GetBytes((int)IRI.Standards.OGC.SFA.WkbGeometryType.Point), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            return result;
        }

        public static byte[] ToWkbPoint(EsriPointM point)
        {
            byte[] result = new byte[29];

            Array.Copy(BitConverter.GetBytes((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr), 0, result, 0, 1);

            Array.Copy(BitConverter.GetBytes((int)IRI.Standards.OGC.SFA.WkbGeometryType.PointM), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            Array.Copy(BitConverter.GetBytes(point.Measure), 0, result, 21, 8);

            return result;
        }

        public static byte[] ToWkbPoint(EsriPointZ point)
        {
            byte[] result = new byte[37];

            Array.Copy(BitConverter.GetBytes((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr), 0, result, 0, 1);

            Array.Copy(BitConverter.GetBytes((int)IRI.Standards.OGC.SFA.WkbGeometryType.PointZM), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            Array.Copy(BitConverter.GetBytes(point.Z), 0, result, 21, 8);

            Array.Copy(BitConverter.GetBytes(point.Measure == ShapeConstants.NoDataValue ? double.NaN : point.Measure), 0, result, 29, 8);

            return result;
        }

        public static byte[] ToWkbMultiPoint(EsriPoint[] points, IRI.Standards.OGC.SFA.WkbGeometryType type)
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)type));

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPoint(points[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbMultiPointM(EsriPoint[] points, double[] measures, IRI.Standards.OGC.SFA.WkbGeometryType type)
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((int)type));

            result.AddRange(BitConverter.GetBytes((int)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPoint(new EsriPointM(points[i].X, points[i].Y, measures[i], points[i].Srid)));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbMultiPointZM(EsriPoint[] points, double[] zValues, double[] measures, IRI.Standards.OGC.SFA.WkbGeometryType type)
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)type));

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPoint(new EsriPointZ(points[i].X, points[i].Y, zValues[i], measures[i], points[i].Srid)));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLinearRing(EsriPoint[] points)
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLinearRingM(EsriPoint[] points, double[] measures)
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLinearRingZM(EsriPoint[] points, double[] zValues, double[] measures)
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(zValues[i]));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLineString(EsriPoint[] points)
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint) IRI.Standards.OGC.SFA.WkbGeometryType.LineString));

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLineStringM(EsriPoint[] points, double[] measures)
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)IRI.Standards.OGC.SFA.WkbGeometryType.LineStringM));

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLineStringZM(EsriPoint[] points, double[] zValues, double[] measures)
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)IRI.Standards.OGC.SFA.WkbGeometryType.LineStringZM));

            result.AddRange(BitConverter.GetBytes((uint)points.Length));

            for (int i = 0; i < points.Length; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(zValues[i]));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }
    }
}
