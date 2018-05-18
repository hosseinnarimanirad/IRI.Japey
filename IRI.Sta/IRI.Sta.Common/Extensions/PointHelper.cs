using IRI.Ham.SpatialBase.Ogc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase.Extensions
{
    public static class PointHelper
    {
        public static byte[] AsWkb(IPoint point)
        {
            byte[] result = new byte[21];

            result[0] = (byte)WkbByteOrder.WkbNdr;

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            return result;
        }
    }
}
