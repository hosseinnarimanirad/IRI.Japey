// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common;


public class VoronoiPoint
{

    //private double m_X;

    //private double m_Y;

    private int m_TriangleCode;

    public double X { get; set; }

    public double Y { get; set; }

    public int TriangleCode
    {
        get { return this.m_TriangleCode; }
    }

    public VoronoiPoint(int triangleCode, Point position)
    {
        this.m_TriangleCode = triangleCode;

        this.X = position.X;

        this.Y = position.Y;

        this.NeigboursCode = new List<int>();
    }

    public List<int> NeigboursCode;

    public override string ToString()
    {
        //StringBuilder temp = new StringBuilder();

        //temp.Append(string.Format("X:{0}, Y:{1}, Neighbours:", X.ToString(), Y.ToString()));

        //foreach (int item in NeigboursCode)
        //{
        //    temp.Append(string.Format(", {0}", item.ToString()));
        //}

        //return temp.ToString();

        return string.Format("X:{0}, Y:{1}, Triangle{2}:", X.ToString(), Y.ToString(), TriangleCode);
    }

    // dar soorate neveshtane hamsayeha dat methode ToString()
    // ba tagire hamsayeha hascode nogata ham avaz mishavad!
    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(VoronoiPoint))
        {
            return false;
        }

        return this.ToString().Equals(obj.ToString());
    }
}
