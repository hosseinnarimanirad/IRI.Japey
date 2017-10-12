using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version04.Primitives
{
    public struct Boundary
    {
        dPoint2D lowerLeft, upperRight;

        int width, height;

        public double Width { get { return width; } }

        public double Height { get { return height; } }

        public double MinX { get { return lowerLeft.X; } }

        public double MinY { get { return lowerLeft.Y; } }

        public Boundary(dPoint2D lowerLeft, dPoint2D upperRight)
        {
            this.lowerLeft = lowerLeft;

            this.upperRight = upperRight;

            this.width = (int)(upperRight.X - lowerLeft.X);

            this.height = (int)(upperRight.Y - lowerLeft.Y);
        }

        public Boundary(dPoint2D lowerLeft, double width, double height)
        {
            this.lowerLeft = lowerLeft;

            this.upperRight = new dPoint2D(lowerLeft.X + width, lowerLeft.Y + height);

            this.width = (int)(upperRight.X - lowerLeft.X);

            this.height = (int)(upperRight.Y - lowerLeft.Y);

        }

        public Boundary GetSubboundary(int row, int column, double width, double height)
        {
            return new Boundary(new dPoint2D(lowerLeft.X + row * width, lowerLeft.Y + column * height), width, height);
        }

    }
}
