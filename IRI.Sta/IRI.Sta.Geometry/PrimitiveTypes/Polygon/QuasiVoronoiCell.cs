﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.Geometry
{
    public class QuasiVoronoiCell : QuasiPolygon
    {
        Point primaryPoint;

        public Point PrimaryPoint
        {
            get { return primaryPoint; }
        }

        public QuasiVoronoiCell(Point primaryPoint, List<int> vertexes)
            : base(vertexes)
        {
            this.primaryPoint = primaryPoint;
        }

        public QuasiVoronoiCell(Point primaryPoint, List<int> vertexes, List<int> neighbours)
            : base(vertexes, neighbours)
        {
            this.primaryPoint = primaryPoint;
        }

        public override string ToString()
        {
            return string.Format("Primary Point:{0}, Vertexes:{1}", PrimaryPoint.ToString(), base.ToString());
        }
    }
}
