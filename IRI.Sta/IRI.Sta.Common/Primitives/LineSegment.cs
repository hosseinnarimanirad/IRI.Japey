using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Primitives
{
    public class LineSegment
    {
        public IPoint Start { get; set; }

        public IPoint End { get; set; }

        public IPoint Middle
        {
            get => new Point((Start.X + End.X) / 2.0, (Start.Y + End.Y) / 2.0);
        }

        public LineSegment(IPoint start, IPoint end)
        {
            this.Start = start;

            this.End = end;
        }
    }
}
