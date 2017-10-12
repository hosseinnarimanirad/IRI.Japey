// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.Geometry
{
    [Serializable]
    public struct Point : IRI.Ket.Geometry.IPoint, IComparable<Point>
    {
        public static PointComparisonPriority ComparisonPriority = PointComparisonPriority.YBased;

        private double m_X, m_Y;

        private readonly int code;

        public double X
        {
            get { return this.m_X; }

            set { this.m_X = value; }
        }

        public double Y
        {
            get { return this.m_Y; }

            set { this.m_Y = value; }
        }

        public Point(double x, double y)
            : this(x, y, -1) { }

        public Point(double x, double y, int code)
        {
            this.m_X = x;

            this.m_Y = y;

            this.code = code;
        }

        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}", X, Y);
        }

        public override int GetHashCode()
        {
            return this.code;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Point))
            {
                return ((Point)obj).X == this.X && ((Point)obj).Y == this.Y;
            }

            return false;
        }

        public  bool AreTheSame(Point point, int precision)
        {
            return Math.Round(this.X, precision).Equals(Math.Round(point.X, precision)) &&
                    Math.Round(this.Y, precision).Equals(Math.Round(point.Y, precision));
        }

        public double CalculateDistance(Point otherPoint)
        {
            double dx = this.X - otherPoint.X;

            double dy = this.Y - otherPoint.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static bool operator ==(Point first, Point second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Point first, Point second)
        {
            return !first.Equals(second);
        }

        #region IComparable<Point> Members

        public int CompareTo(Point other)
        {
            int tempValue;

            if (ComparisonPriority == PointComparisonPriority.XBased)
            {
                tempValue = this.X.CompareTo(other.X);

                if (tempValue == 0)
                {
                    tempValue = this.Y.CompareTo(other.Y);
                }
            }
            else
            {
                tempValue = this.Y.CompareTo(other.Y);

                if (tempValue == 0)
                {
                    tempValue = this.X.CompareTo(other.X);
                }
            }

            return tempValue;
        }

        #endregion
    }
}