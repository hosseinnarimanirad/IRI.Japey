using IRI.Ham.SpatialBase.Ogc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase
{
    public class Point : IPoint
    {

        public double X { get; set; }

        public double Y { get; set; }

        public Point(double x, double y)
        {
            this.X = x;

            this.Y = y;
        }

        public Point()
        {
            this.X = 0;

            this.Y = 0;
        }

        public static double EuclideanDistance(IPoint firstPoint, IPoint secondPoint)
        {
            return
                Math.Sqrt((firstPoint.X - secondPoint.X) * (firstPoint.X - secondPoint.X) +
                    (firstPoint.Y - secondPoint.Y) * (firstPoint.Y - secondPoint.Y));
        }

        public static double GetDistance(Point first, Point second)
        {
            return Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}, {1}", this.X, this.Y);
        }
         
        public bool IsNaN()
        {
            return double.IsNaN(X) || double.IsNaN(Y);
        }

        public string AsExactString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:G17} {1:G17}", this.X, this.Y);
        }

        public bool AreExactlyTheSame(object obj)
        {
            if (obj.GetType() != typeof(Point))
            {
                return false;
            }

            return this.AsExactString() == ((Point)obj).AsExactString();
        }

        public double DistanceTo(IPoint point)
        {
            return GetDistance(this, new Point(point.X, point.Y));
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p2.X - p1.X, p2.Y - p1.Y);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point NaN { get; } = new Point(double.NaN, double.NaN);

        public static Point PositiveInfinity { get; } = new Point(double.PositiveInfinity, double.PositiveInfinity);

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Point))
            {
                Point temp = (Point)obj;

                return temp.X == this.X && temp.Y == this.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public byte[] AsWkb()
        {
            return Extensions.PointHelper.AsWkb(this);
        }


        public static Point Parse(double[] values, bool isLongitudeFirst)
        {
            if (values == null || values.Count() != 2)
            {
                throw new NotImplementedException();
            }

            if (isLongitudeFirst)
            {
                return new Point(values[0], values[1]);
            }
            else
            {
                return new Point(values[1], values[0]);
            }
        }


        //public Microsoft.SqlServer.Types.SqlGeometry AsSqlGeometry()
        //{
        //    return Microsoft.SqlServer.Types.SqlGeometry.Parse(
        //        new System.Data.SqlTypes.SqlString(string.Format("POINT({0} {1})", this.X.ToString(), this.Y.ToString())));
        //}
    }
}
