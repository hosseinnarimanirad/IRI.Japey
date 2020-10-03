using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class LineSegment<T> where T : IPoint, new()
    {
        public T Start { get; set; }

        public T End { get; set; }

        public T Middle
        {
            get => new T() { X = (Start.X + End.X) / 2.0, Y = (Start.Y + End.Y) / 2.0 };
        }

        public LineSegment(T start, T end)
        {
            this.Start = start;

            this.End = end;
        }
    }
}
