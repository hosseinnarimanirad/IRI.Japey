using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Primitives
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
}
