using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version05.Primitives
{
    public struct Point2D
    {
        double x, y;

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public Point2D(double x, double y)
        {
            this.x = x;

            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y:{1}", this.x, this.y);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Point2D))
            {
                Point2D temp = (Point2D)obj;

                return temp.X == this.X && temp.Y == this.Y;
            }

            return false;
        }
    }

    //public struct dPoint2D
    //{
    //    double x, y;

    //    public double X
    //    {
    //        get { return this.x; }
    //        set { this.x = value; }
    //    }

    //    public double Y
    //    {
    //        get { return this.y; }
    //        set { this.y = value; }
    //    }

    //    public dPoint2D(double x, double y)
    //    {
    //        this.x = x;

    //        this.y = y;
    //    }

    //    public override string ToString()
    //    {
    //        return string.Format("X: {0}, Y:{1}", this.x, this.y);
    //    }
    //}
}
