// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj
 
using IRI.Msh.Common.Primitives;
using System;

namespace IRI.Ket.DigitalTerrainModeling;

[Serializable]
public struct AttributedPoint : IPoint
{
    private double m_X, m_Y, m_Value;

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

    public double Value
    {
        get { return this.m_Value; }

        set { this.m_Value = value; }
    }

    public AttributedPoint(double x, double y, double value)
        : this(x, y, value, -1){}

    public AttributedPoint(double x, double y, double value, int code)
    {
        this.m_X = x;

        this.m_Y = y;

        this.m_Value = value;

        this.code = code;
    }

    public override string ToString()
    {
        return string.Format("Coordinate: X:{0}, Y:{1}; Attribute:{2}", X, Y, Value);
    }

    public override int GetHashCode()
    {
        return this.code;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(AttributedPoint))
        {
            return ((AttributedPoint)obj).X == this.X &&
                    ((AttributedPoint)obj).Y == this.Y &&
                    ((AttributedPoint)obj).Value == this.Value;
        }

        return false;
    }

    public  bool AreTheSame(AttributedPoint point, int precision)
    {
        return Math.Round(this.X, precision).Equals(Math.Round(point.X, precision)) &&
                Math.Round(this.Y, precision).Equals(Math.Round(point.Y, precision)) &&
                Math.Round(this.Value, precision).Equals(Math.Round(point.Value, precision));
    }

    public double CalculateDistance(AttributedPoint nextPoint)
    {
        return Math.Sqrt((this.X - nextPoint.X) * (this.X - nextPoint.X) + (this.Y - nextPoint.Y) * (this.Y - nextPoint.Y));
    }

    public bool AreExactlyTheSame(object obj)
    {
        throw new NotImplementedException();
    }

    public double DistanceTo(IPoint point)
    {
        throw new NotImplementedException();
    }

    public byte[] AsWkb()
    {
        throw new NotImplementedException();
    }


    public byte[] AsSqlServerNativeBinary()
    {
        // Option #3
        Span<byte> buffer = stackalloc byte[16];  // Stack-allocated, no heap allocation

        BitConverter.TryWriteBytes(buffer.Slice(0, 8), X);

        BitConverter.TryWriteBytes(buffer.Slice(8, 8), Y);
         
        return buffer.ToArray();  // Only allocates when creating final array
    }

    public bool IsNaN()
    {
        return double.IsNaN(X) || double.IsNaN(Y);
    }
}
