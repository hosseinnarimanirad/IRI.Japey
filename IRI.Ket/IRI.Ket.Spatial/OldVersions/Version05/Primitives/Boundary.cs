using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version05.Primitives
{
    public struct Boundary
    {
        Point2D lowerLeft, upperRight;

        double width, height;

        public double Width { get { return width; } }

        public double Height { get { return height; } }

        public double MinX { get { return lowerLeft.X; } }

        public double MinY { get { return lowerLeft.Y; } }

        public Boundary(Point2D lowerLeft, Point2D upperRight)
        {
            this.lowerLeft = lowerLeft;

            this.upperRight = upperRight;

            this.width = (upperRight.X - lowerLeft.X);

            this.height = (upperRight.Y - lowerLeft.Y);
        }

        public Boundary(Point2D lowerLeft, double width, double height)
        {
            this.lowerLeft = lowerLeft;

            this.upperRight = new Point2D(lowerLeft.X + width, lowerLeft.Y + height);

            this.width = (upperRight.X - lowerLeft.X);

            this.height = (upperRight.Y - lowerLeft.Y);

        }

        public Boundary GetSubboundary(int row, int column, double width, double height)
        {
            return new Boundary(new Point2D(lowerLeft.X + row * width, lowerLeft.Y + column * height), width, height);
        }

        public override string ToString()
        {
            return string.Format("LL:{0}, UR:{1}", lowerLeft.ToString(), upperRight.ToString());
        }

        public bool DoseContainsPoint(Point2D point)
        {
            return
                point.X <= this.upperRight.X &&
                point.X >= this.lowerLeft.X &&
                point.Y <= this.upperRight.Y &&
                point.Y >= this.lowerLeft.Y;

        }
    }
}
