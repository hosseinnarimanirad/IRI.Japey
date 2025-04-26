// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common;

public class Triangle
{
    private Point firstPoint, secondPoint, thirdPoint;

    private int firstSecondNeighbour, secondThirdNeighbour, thirdFirstNeighbour;

    //directed arrow from first to second edge
    public static readonly int firstEdgeWeight = 1;

    //directed arrow from second to third edge
    public static readonly int secondEdgeWeight = 3;

    //directed arrow from third to first edge
    public static readonly int thirdEdgeWeight = 9;

    public Point FirstPoint
    {
        get { return firstPoint; }
        set { firstPoint = value; }
    }

    public Point SecondPoint
    {
        get { return secondPoint; }
        set { secondPoint = value; }
    }

    public Point ThirdPoint
    {
        get { return thirdPoint; }
        set { thirdPoint = value; }
    }

    public double FirstSecondSide
    {
        get
        {
            return FirstPoint.DistanceTo(SecondPoint);
            //return ComputationalGeometry.CalculateDistance(FirstPoint, SecondPoint);
        }
    }

    public double SecondThirdSide
    {
        get
        {
            return SecondPoint.DistanceTo(ThirdPoint);
            //return ComputationalGeometry.CalculateDistance(SecondPoint, ThirdPoint);
        }
    }

    public double ThirdFirstSide
    {
        get
        {
            return ThirdPoint.DistanceTo(FirstPoint);
            //return ComputationalGeometry.CalculateDistance(ThirdPoint, FirstPoint);
        }
    }

    /// <summary>
    /// Angle in radian
    /// </summary>
    public double FirstAngle
    {
        get
        {
            double tempValue = (SecondThirdSide * SecondThirdSide - FirstSecondSide * FirstSecondSide - ThirdFirstSide * ThirdFirstSide)
                                /
                                (2 * FirstSecondSide * ThirdFirstSide);

            return Math.Acos(tempValue);
        }
    }

    /// <summary>
    /// Angle in radian
    /// </summary>
    public double SecondAngle
    {
        get
        {
            double tempValue = (ThirdFirstSide * ThirdFirstSide - FirstSecondSide * FirstSecondSide - SecondThirdSide * SecondThirdSide)
                                /
                                (2 * FirstSecondSide * SecondThirdSide);

            return Math.Acos(tempValue);
        }
    }

    /// <summary>
    /// Angle in radian
    /// </summary>
    public double ThirdAngle
    {
        get
        {
            double tempValue = (FirstSecondSide * FirstSecondSide - SecondThirdSide * SecondThirdSide - ThirdFirstSide * ThirdFirstSide)
                                /
                                (2 * SecondThirdSide * ThirdFirstSide);

            return Math.Acos(tempValue);
        }
    }

    public int FirstSecondNeighbour
    {
        get { return this.firstSecondNeighbour; }
        set { this.firstSecondNeighbour = value; }
    }

    public int SecondThirdNeighbour
    {
        get { return this.secondThirdNeighbour; }
        set { this.secondThirdNeighbour = value; }
    }

    public int ThirdFirstNeighbour
    {
        get { return this.thirdFirstNeighbour; }
        set { this.thirdFirstNeighbour = value; }
    }

    public Triangle()
    {
        this.firstSecondNeighbour = -1;

        this.secondThirdNeighbour = -1;

        this.thirdFirstNeighbour = -1;
    }

    public Triangle(Point first, Point second, Point third)
    {
        this.firstPoint = first;

        this.secondPoint = second;

        this.thirdPoint = third;

        this.firstSecondNeighbour = -1;

        this.secondThirdNeighbour = -1;

        this.thirdFirstNeighbour = -1;
    }

    public override string ToString()
    {
        return string.Format("First:{0}, Second:{1}, Third:{2}", firstPoint.ToString(), secondPoint.ToString(), thirdPoint.ToString());
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(Triangle))
        {
            Triangle tempTriangle = (Triangle)obj;

            return tempTriangle.FirstPoint.Equals(this.FirstPoint) &&
                    tempTriangle.SecondPoint.Equals(this.SecondPoint) &&
                    tempTriangle.ThirdPoint.Equals(this.ThirdPoint);
        }

        return false;
    }

    public double CalculateArea()
    {
        double firstSecondLength = this.FirstPoint.DistanceTo(this.SecondPoint);

        double secondThirdLength = this.SecondPoint.DistanceTo(this.ThirdPoint);

        double thirdFirstLength = this.ThirdPoint.DistanceTo(this.FirstPoint);

        double semiPerimeter = (firstSecondLength + secondThirdLength + thirdFirstLength) / 2;

        return Math.Sqrt(semiPerimeter *
                        (semiPerimeter - firstSecondLength) *
                        (semiPerimeter - secondThirdLength) *
                        (semiPerimeter - thirdFirstLength));
    }

    public double CalculatePrimeter()
    {
        //double firstSecondLength = this.FirstPoint.CalculateDistance(this.SecondPoint);

        //double secondThirdLength = this.SecondPoint.CalculateDistance(this.ThirdPoint);

        //double thirdFirstLength = this.ThirdPoint.CalculateDistance(this.FirstPoint);

        return (this.FirstSecondSide + this.SecondThirdSide + this.ThirdFirstSide);
    }

    //public bool HasThePoint(int pointCode)
    //{
    //    return this.first == pointCode ||
    //            this.second == pointCode ||
    //            this.third == pointCode;
    //}

    //public TriangleRelation GetRelationTo(QuasiTriangle triangle)
    //{
    //    int hasTheFirst = System.Convert.ToInt32(triangle.HasThePoint(this.First));

    //    int hasTheSecond = System.Convert.ToInt32(triangle.HasThePoint(this.Second));

    //    int hasTheThird = System.Convert.ToInt32(triangle.HasThePoint(this.Third));

    //    return (TriangleRelation)((hasTheFirst * (int)TriangleRelation.MeetAtFirst) +
    //                                hasTheSecond * (int)TriangleRelation.MeetAtSecond +
    //                                hasTheThird * (int)TriangleRelation.MeetAtThird);
    //}

    //public bool HasTheEdge(QuasiEdge edge)
    //{
    //    return (this.HasThePoint(edge.First) && this.HasThePoint(edge.Second));
    //}

    //public int GetThirdPoint(QuasiEdge edge)
    //{
    //    if (!HasTheEdge(edge))
    //    {
    //        throw new NotImplementedException();
    //    }

    //    if (this.first != edge.First && this.first != edge.Second)
    //    {
    //        return this.first;
    //    }
    //    else if (this.Second != edge.First && this.Second != edge.Second)
    //    {
    //        return this.Second;
    //    }
    //    else if (this.third != edge.First && this.third != edge.Second)
    //    {
    //        return this.third;
    //    }
    //    else
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public int GetNeighbour(QuasiEdge commonEdge)
    //{
    //    if (!HasTheEdge(commonEdge))
    //    {
    //        throw new NotImplementedException();
    //    }

    //    if (this.first == commonEdge.First)
    //    {
    //        if (this.second == commonEdge.Second)
    //        {
    //            return this.FirstSecondNeighbour;
    //        }
    //        else if (this.third == commonEdge.Second)
    //        {
    //            return this.ThirdFirstNeighbour;
    //        }
    //    }
    //    else if (this.second == commonEdge.First)
    //    {
    //        if (this.first == commonEdge.Second)
    //        {
    //            return this.FirstSecondNeighbour;
    //        }
    //        else if (this.third == commonEdge.Second)
    //        {
    //            return this.SecondThirdNeighbour;
    //        }
    //    }
    //    else if (this.third == commonEdge.First)
    //    {
    //        if (this.first == commonEdge.Second)
    //        {
    //            return this.ThirdFirstNeighbour;
    //        }
    //        else if (this.second == commonEdge.Second)
    //        {
    //            return this.SecondThirdNeighbour;
    //        }
    //    }

    //    throw new NotImplementedException();
    //}

    /// <summary>
    /// Calculate the intersection of the triangle's medians
    /// </summary>
    /// <returns></returns>
    public Point CalculateCentroid()
    {
        double x = (this.FirstPoint.X + this.SecondPoint.X + this.ThirdPoint.X) / 3;

        double y = (this.FirstPoint.Y + this.SecondPoint.Y + this.ThirdPoint.Y) / 3;

        return new Point(x, y);
    }

    /// <summary>
    /// Calculate the intersection of perpendicular bisectors
    /// </summary>
    /// <returns></returns>
    public Point CalculateCircumcenter()
    {
        double x1 = this.FirstPoint.X; double y1 = this.FirstPoint.Y;

        double x2 = this.SecondPoint.X; double y2 = this.SecondPoint.Y;

        double x3 = this.ThirdPoint.X; double y3 = this.ThirdPoint.Y;

        double temp = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));

        double tempX = ((y1 * y1 + x1 * x1) * (y2 - y3) + (y2 * y2 + x2 * x2) * (y3 - y1) + (y3 * y3 + x3 * x3) * (y1 - y2)) / temp;

        double tempY = ((y1 * y1 + x1 * x1) * (x3 - x2) + (y2 * y2 + x2 * x2) * (x1 - x3) + (y3 * y3 + x3 * x3) * (x2 - x1)) / temp;

        return new Point(tempX, tempY);
    }

    /// <summary>
    /// Calculate the intersection of triangle's bisectors
    /// </summary>
    /// <returns></returns>
    public Point CalculateIncenter()
    {
        double a = this.SecondThirdSide;

        double b = this.ThirdFirstSide;

        double c = this.FirstSecondSide;

        double primeter = a + b + c;

        return new Point((a * this.FirstPoint.X + b * this.SecondPoint.X + c * this.ThirdPoint.X) / primeter,
                            (a * this.FirstPoint.Y + b * this.SecondPoint.Y + c * this.ThirdPoint.Y) / primeter);
    }
    //((a2 + b2 − c2)(a2  − b2 + c2):(a2 + b2  − c2)( − a2 + b2 + c2):(a2 − b2 + c2)( − a2 + b2 + c2)).

    //public Point CalculateOrthocenter()
    //{
    //     double aPrime = this.SecondThirdSide*this.SecondThirdSide;

    //    double bPrime = this.ThirdFirstSide* this.ThirdFirstSide;

    //    double cPrime = this.FirstSecondSide*this.FirstSecondSide;



    //}
}

