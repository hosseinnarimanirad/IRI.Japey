// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.Geometry
{
    public class VoronoiCell : ConvexPolygon
    {
        public Point PrimaryPoint { get; set; }

        public VoronoiCell()
            : base(new PointCollection())
        {
            this.PrimaryPoint = new Point();
        }

        public VoronoiCell(Point primaryPoint)
            : base(new PointCollection())
        {
            this.PrimaryPoint = primaryPoint;
        }

        public VoronoiCell(Point primaryPoint, PointCollection vertexes)
            : base(vertexes)
        {
            this.PrimaryPoint = primaryPoint;
        }

        public VoronoiCell(Point primaryPoint, PointCollection vertexes, List<int> neighbours)
            : base(vertexes, neighbours)
        {
            this.PrimaryPoint = primaryPoint;
        }

        public override string ToString()
        {
            return string.Format("Primary Point:{0}, Vertexes:{1}", PrimaryPoint.ToString(), base.ToString());
        }

        public override int GetHashCode()
        {
            return this.PrimaryPoint.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(VoronoiCell) && (obj.ToString().Equals(this.ToString()));
        }

        public void Clip(Point firstPointLine, Point secondPointLine)
        {
            List<int> edgeIndexes;

            List<Point> intersections = Intersect(firstPointLine, secondPointLine, out edgeIndexes);

            if (intersections.Count < 2)
            {
                return;
            }

            List<Point> temp1 = new List<Point>(); List<Point> temp2 = new List<Point>();

            temp1.Add(intersections[0]);

            for (int i = edgeIndexes[0] + 1; i < edgeIndexes[1]; i++)
            {
                temp1.Add(Vertexes[i]);
            }

            temp1.Add(intersections[1]);

            temp2.Add(intersections[1]);

            for (int i = edgeIndexes[1] + 1; i < edgeIndexes[0] + Vertexes.Count - 1 - edgeIndexes[1]; i++)
            {
                int j = (i + 1) % Vertexes.Count;

                temp2.Add(Vertexes[j]);
            }

            temp2.Add(intersections[0]);
        }
    }
}
