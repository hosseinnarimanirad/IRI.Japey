using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Msh.Common.Ogc;
using IRI.Msh.Common.Primitives;

namespace IRI.Msh.Common.Ogc
{
    public static class OgcWkbMapFunctions
    {
        public static byte[] ToWkbPoint<T>(T point) where T : IPoint
        {
            byte[] result = new byte[21];

            result[0] = (byte)WkbByteOrder.WkbNdr;

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            return result;
        }

        public static byte[] ToWkbPointM<T>(T point, double measure) where T : IPoint
        {
            byte[] result = new byte[29];

            Array.Copy(BitConverter.GetBytes((byte)WkbByteOrder.WkbNdr), 0, result, 0, 1);

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.PointM), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            Array.Copy(BitConverter.GetBytes(measure), 0, result, 21, 8);

            return result;
        }

        public static byte[] ToWkbPointZM<T>(T point, double z, double measure) where T : IPoint
        {
            byte[] result = new byte[37];

            Array.Copy(BitConverter.GetBytes((byte)WkbByteOrder.WkbNdr), 0, result, 0, 1);

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.PointZM), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            Array.Copy(BitConverter.GetBytes(z), 0, result, 21, 8);

            Array.Copy(BitConverter.GetBytes(measure == EsriConstants.NoDataValue ? double.NaN : measure), 0, result, 29, 8);

            return result;
        }

        public static byte[] ToWkbMultiPoint<T>(IReadOnlyList<T> points) where T : IPoint
        {
            List<byte> result = new List<byte>
            {
                (byte)WkbByteOrder.WkbNdr
            };

            result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.MultiPoint));

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPoint(points[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbMultiPointM<T>(IReadOnlyList<T> points, double[] measures) where T : IPoint
        {
            List<byte> result = new List<byte>
            {
                (byte)WkbByteOrder.WkbNdr
            };

            result.AddRange(BitConverter.GetBytes((int)WkbGeometryType.MultiPointM));

            result.AddRange(BitConverter.GetBytes((int)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPointM(points[i], measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbMultiPointZM<T>(IReadOnlyList<T> points, double[] zValues, double[] measures) where T : IPoint
        {
            List<byte> result = new List<byte>
            {
                (byte)WkbByteOrder.WkbNdr
            };

            result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.MultiPointZM));

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPointZM(points[i], zValues[i], measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLinearRing<T>(IReadOnlyList<T> points) where T : IPoint
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLinearRingM<T>(IReadOnlyList<T> points, double[] measures) where T : IPoint
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLinearRingZM<T>(IReadOnlyList<T> points, double[] zValues, double[] measures) where T : IPoint
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(zValues[i]));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLineString<T>(IReadOnlyList<T> points) where T : IPoint
        {
            List<byte> result = new List<byte>
            {
                (byte)WkbByteOrder.WkbNdr
            };

            result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.LineString));

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLineStringM<T>(IReadOnlyList<T> points, double[] measures) where T : IPoint
        {
            List<byte> result = new List<byte>
            {
                (byte)WkbByteOrder.WkbNdr
            };

            result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.LineStringM));

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
            {
                result.AddRange(BitConverter.GetBytes(points[i].X));

                result.AddRange(BitConverter.GetBytes(points[i].Y));

                result.AddRange(BitConverter.GetBytes(measures[i]));
            }

            return result.ToArray();
        }

        public static byte[] ToWkbLineStringZM<T>(IReadOnlyList<T> points, double[] zValues, double[] measures) where T : IPoint
        {
            List<byte> result = new List<byte>
            {
                (byte)WkbByteOrder.WkbNdr
            };

            result.AddRange(BitConverter.GetBytes((uint)WkbGeometryType.LineStringZM));

            result.AddRange(BitConverter.GetBytes((uint)points.Count));

            for (int i = 0; i < points.Count; i++)
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
