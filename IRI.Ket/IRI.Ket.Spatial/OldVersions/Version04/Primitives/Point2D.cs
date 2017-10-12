using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version04.Primitives
{
    public struct Point2D
    {
        int x, y;

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public Point2D(int x, int y)
        {
            this.x = x;

            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y:{1}", this.x, this.y);
        }
    }

    public struct dPoint2D
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

        public dPoint2D(double x, double y)
        {
            this.x = x;

            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y:{1}", this.x, this.y);
        }
    }
}
