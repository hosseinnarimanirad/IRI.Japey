using IRI.Msh.Common.Ogc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class Point3D : IPoint, IHasZ
    {
        private double _x;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private double _z;

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }


        public Point3D(double x, double y, double z)
        {
            this.X = x;

            this.Y = y;

            this.Z = z;
        }

        public Point3D()
        {
            this.X = 0;

            this.Y = 0;

            this.Z = 0;
        }

        public static Point3D NaN
        {
            get { return new Point3D(double.NaN, double.NaN, double.NaN); }
        }

        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static explicit operator Point(Point3D point)
        {
            return new Point(point.X, point.Y);
        }


        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}, {1}, {2}", this.X, this.Y, this.Z);
        }

        public string AsExactString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:G17} {1:G17} {2:G17}", this.X, this.Y, this.Z);
        }

        public bool AreExactlyTheSame(object obj)
        {
            if (obj.GetType() != typeof(Point3D))
            {
                return false;
            }

            return this.AsExactString() == ((Point3D)obj).AsExactString();
        }
        public static double GetDistance(Point3D first, Point3D second)
        {
            return Math.Sqrt((first.X - second.X) * (first.X - second.X) +
                                (first.Y - second.Y) * (first.Y - second.Y) +
                                (first.Z - second.Z) * (first.Z - second.Z));
        }

        public double DistanceTo(IPoint point)
        {
            if (point is Point3D)
            {
                return GetDistance(this, (Point3D)point);
            }
            else
            {
                return IRI.Msh.Common.Analysis.SpatialUtility.GetEuclideanDistance(new Point(this.X, this.Y), new Point(point.X, point.Y));
            }
        }


        public byte[] AsWkb()
        {
            byte[] result = new byte[29];

            result[0] = (byte)WkbByteOrder.WkbNdr;

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(Y), 0, result, 13, 8);

            Array.Copy(BitConverter.GetBytes(Z), 0, result, 21, 8);

            return result;
        }


    }
}
