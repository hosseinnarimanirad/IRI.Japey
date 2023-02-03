using IRI.Msh.Common.Helpers;
using IRI.Msh.Common.Ogc;
using IRI.Msh.CoordinateSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace IRI.Msh.Common.Primitives
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

        //public static double GetDistance(Point first, Point second)
        //{
        //    return Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
        //}

        //public static double EuclideanDistance(IPoint first, IPoint second)
        //{
        //    return
        //        Math.Sqrt((first.X - second.X) * (first.X - second.X) +
        //                    (first.Y - second.Y) * (first.Y - second.Y));
        //}

        public static Point FromPolar(double radius, double angleInRadian)
        {
            return new Point(
                radius * Math.Cos(angleInRadian), 
                radius * Math.Sin(angleInRadian));
        }

        // https://medium.com/swlh/calculating-the-distance-between-two-points-on-earth-bac5cd50c840
        // https://www.movable-type.co.uk/scripts/latlong.html
        // https://stormconsultancy.co.uk/blog/storm-news/the-haversine-formula-in-c-and-sql/
        // https://social.msdn.microsoft.com/Forums/sqlserver/en-US/6a0cc084-5056-4f97-9978-a5f88cb57d0f/stdistance-vs-doing-the-math-manually?forum=sqlspatial
        // https://stackoverflow.com/questions/42237521/sql-server-geography-stdistance-function-is-returning-big-difference-than-other
        // https://stackoverflow.com/questions/27708490/haversine-formula-definition-for-sql
        // https://medium.com/swlh/calculating-the-distance-between-two-points-on-earth-bac5cd50c840
        public static double SphericalDistance(IPoint firstPoint, IPoint secondPoint)
        {
            //var radius = 6371008.8; // in meters

            //var radius = 6368045.28;
            //var radius = 6367538.5803727582

            var radius = (Ellipsoids.WGS84.SemiMajorAxis.Value + Ellipsoids.WGS84.SemiMinorAxis.Value) / 2.0;

            //            Haversine
            //formula: 	a = sin²(Δφ / 2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ / 2)
            //c = 2 ⋅ atan2( √a, √(1−a) )
            //d = R ⋅ c
            var phi1 = firstPoint.Y * Math.PI / 180.0;

            var phi2 = secondPoint.Y * Math.PI / 180.0;

            var a = Ellipsoids.WGS84.SemiMajorAxis.Value;
            var b = Ellipsoids.WGS84.SemiMinorAxis.Value;
            var meanPhi = (phi1 + phi2) / 2.0;
            var newR = Math.Sqrt(a * a * Math.Cos(meanPhi) * Math.Cos(meanPhi) + b * b * Math.Sin(meanPhi) * Math.Sin(meanPhi));

            var deltaPhi = (secondPoint.Y - firstPoint.Y) * Math.PI / 180.0;

            var deltaLambda = (secondPoint.X - firstPoint.X) * Math.PI / 180.0;

            //var temp = radius * Math.Acos(Math.Cos(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda) + Math.Sin(phi1) * Math.Sin(phi2)); //72092.799646276282

            var haversine = Math.Sin(deltaPhi / 2.0) * Math.Sin(deltaPhi / 2.0) +
                            Math.Cos(phi1) * Math.Cos(phi2) * Math.Sin(deltaLambda / 2.0) * Math.Sin(deltaLambda / 2.0);

            var c = 2.0 * Math.Atan2(Math.Sqrt(haversine), Math.Sqrt(1 - haversine));

            //var c2 = 2.0 * Math.Asin(Math.Min(1, Math.Sqrt(haversine)));
            //var t3 = radius * c2;

            return newR * c; // in meters
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

        public bool AreTheSame(Point point, int precision)
        {
            return Math.Round(this.X, precision).Equals(Math.Round(point.X, precision)) &&
                    Math.Round(this.Y, precision).Equals(Math.Round(point.Y, precision));
        }

        //public double DistanceTo(IPoint point)
        //{
        //    return GetDistance(this, new Point(point.X, point.Y));
        //}

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
            if (obj?.GetType() == typeof(Point))
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
            byte[] result = new byte[21];

            result[0] = (byte)WkbByteOrder.WkbNdr;

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, BaseConversionHelper.IntegerSize);

            Array.Copy(BitConverter.GetBytes(X), 0, result, 5, BaseConversionHelper.DoubleSize);

            Array.Copy(BitConverter.GetBytes(Y), 0, result, 13, BaseConversionHelper.DoubleSize);

            return result;
        }

        public virtual string AsWkt()
        {
            return FormattableString.Invariant($"POINT({X.ToInvariantString()} {Y.ToInvariantString()}");
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

        public static T Parse<T>(double[] values, bool isLongitudeFirst) where T : IPoint, new()
        {
            var point = Parse(values, isLongitudeFirst);

            T result = new T()
            {
                X = point.X,
                Y = point.Y
            };

            return result;
        }

        public static Point Create(double x, double y)
        {
            return new Point(x, y);
        }

        //public static bool operator ==(Point first, Point second)
        //{
        //    return first.Equals(second);
        //}

        //public static bool operator !=(Point first, Point second)
        //{
        //    return !first.Equals(second);
        //}


        //public Microsoft.SqlServer.Types.SqlGeometry AsSqlGeometry()
        //{
        //    return Microsoft.SqlServer.Types.SqlGeometry.Parse(
        //        new System.Data.SqlTypes.SqlString(string.Format("POINT({0} {1})", this.X.ToString(), this.Y.ToString())));
        //}
    }
}
