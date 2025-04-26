//// besmellahe rahmane rahim
//// Allahomma ajjel le-valiyek al-faraj

//using System;
//using IRI.Sta.Common.Ogc;
//using IRI.Sta.Common.Primitives;

//namespace IRI.Sta.Common;

//[Serializable]
//public struct Point : IPoint, IComparable<Point>
//{
//    public static PointComparisonPriority ComparisonPriority = PointComparisonPriority.YBased;

//    private double m_X, m_Y;

//    private readonly int code;

//    public double X
//    {
//        get { return this.m_X; }

//        set { this.m_X = value; }
//    }

//    public double Y
//    {
//        get { return this.m_Y; }

//        set { this.m_Y = value; }
//    }

//    public Point(double x, double y)
//        : this(x, y, -1) { }

//    public Point(double x, double y, int code)
//    {
//        this.m_X = x;

//        this.m_Y = y;

//        this.code = code;
//    }

//    public override string ToString()
//    {
//        return string.Format("X:{0}, Y:{1}", X, Y);
//    }

//    public override int GetHashCode()
//    {
//        return this.code;
//    }

//    public override bool Equals(object obj)
//    {
//        if (obj.GetType() == typeof(Point))
//        {
//            return ((Point)obj).X == this.X && ((Point)obj).Y == this.Y;
//        }

//        return false;
//    }

//    public  bool AreTheSame(Point point, int precision)
//    {
//        return Math.Round(this.X, precision).Equals(Math.Round(point.X, precision)) &&
//                Math.Round(this.Y, precision).Equals(Math.Round(point.Y, precision));
//    }

//    public double CalculateDistance(Point otherPoint)
//    {
//        double dx = this.X - otherPoint.X;

//        double dy = this.Y - otherPoint.Y;

//        return Math.Sqrt(dx * dx + dy * dy);
//    }

//    public static bool operator ==(Point first, Point second)
//    {
//        return first.Equals(second);
//    }

//    public static bool operator !=(Point first, Point second)
//    {
//        return !first.Equals(second);
//    }

//    #region IComparable<Point> Members

//    public int CompareTo(Point other)
//    {
//        int tempValue;

//        if (ComparisonPriority == PointComparisonPriority.XBased)
//        {
//            tempValue = this.X.CompareTo(other.X);

//            if (tempValue == 0)
//            {
//                tempValue = this.Y.CompareTo(other.Y);
//            }
//        }
//        else
//        {
//            tempValue = this.Y.CompareTo(other.Y);

//            if (tempValue == 0)
//            {
//                tempValue = this.X.CompareTo(other.X);
//            }
//        }

//        return tempValue;
//    }

//    #endregion

//    public byte[] AsWkb()
//    {
//        byte[] result = new byte[21];

//        result[0] = (byte)WkbByteOrder.WkbNdr;

//        Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, 4);

//        Array.Copy(BitConverter.GetBytes(X), 0, result, 5, 8);

//        Array.Copy(BitConverter.GetBytes(Y), 0, result, 13, 8);

//        return result;
//    }

//    public byte[] AsSqlServerNativeBinary()
//    {
//        // Option #1
//        //byte[] result = new byte[16];

//        //Array.Copy(BitConverter.GetBytes(X), 0, result, 0, 8);

//        //Array.Copy(BitConverter.GetBytes(Y), 0, result, 8, 8);

//        //return result;


//        // Option #2
//        //byte[] result = new byte[16];

//        //BitConverter.TryWriteBytes(result.AsSpan(0, 8), X);

//        //BitConverter.TryWriteBytes(result.AsSpan(8, 8), Y);

//        //return result;

         
//        // Option #3
//        Span<byte> buffer = stackalloc byte[16];  // Stack-allocated, no heap allocation

//        BitConverter.TryWriteBytes(buffer.Slice(0, 8), X);

//        BitConverter.TryWriteBytes(buffer.Slice(8, 8), Y);

//        return buffer.ToArray();  // Only allocates when creating final array
//    }

//    public string AsExactString()
//    {
//        return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:G17} {1:G17}", this.X, this.Y);
//    }

//    public bool AreExactlyTheSame(object obj)
//    {
//        if (obj.GetType() != typeof(Point))
//        {
//            return false;
//        }

//        return this.AsExactString() == ((Point)obj).AsExactString();
//    }

//    public double DistanceTo(IPoint point)
//    {
//        double dx = this.X - point.X;

//        double dy = this.Y - point.Y;

//        return Math.Sqrt(dx * dx + dy * dy); 
//    }

//    public bool IsNaN()
//    {
//        return double.IsNaN(X) || double.IsNaN(Y);
//    }

//    //public static double GetDistance(Point first, Point second)
//    //{
//    //    return Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
//    //}

//}