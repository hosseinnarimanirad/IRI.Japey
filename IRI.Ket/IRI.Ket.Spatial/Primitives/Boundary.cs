using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Spatial.Primitives
{
    public struct Boundary
    {
        Point lowerLeft, upperRight;

        double width, height;

        public double Width { get { return width; } }

        public double Height { get { return height; } }

        public double MinX { get { return lowerLeft.X; } }

        public double MinY { get { return lowerLeft.Y; } }

        public Boundary(Point lowerLeft, Point upperRight)
        {
            this.lowerLeft = lowerLeft;

            this.upperRight = upperRight;

            this.width = (upperRight.X - lowerLeft.X);

            this.height = (upperRight.Y - lowerLeft.Y);
        }

        public Boundary(Point lowerLeft, double width, double height)
        {
            this.lowerLeft = lowerLeft;

            this.upperRight = new Point(lowerLeft.X + width, lowerLeft.Y + height);

            this.width = (upperRight.X - lowerLeft.X);

            this.height = (upperRight.Y - lowerLeft.Y);

        }

        public Boundary GetSubboundary(int row, int column, double width, double height)
        {
            return new Boundary(new Point(lowerLeft.X + row * width, lowerLeft.Y + column * height), width, height);
        }

        public void MoveBoundary(double deltaX, double deltaY)
        {
            this.lowerLeft.X += deltaX;

            this.upperRight.X += deltaX;

            this.lowerLeft.Y += deltaY;

            this.upperRight.Y += deltaY;
        }

        public override string ToString()
        {
            return string.Format("LL:{0}, UR:{1}", lowerLeft.ToString(), upperRight.ToString());
        }

        public bool DoseContainsPoint(Point point)
        {
            return
                point.X <= this.upperRight.X &&
                point.X >= this.lowerLeft.X &&
                point.Y <= this.upperRight.Y &&
                point.Y >= this.lowerLeft.Y;

        }
    }
}
