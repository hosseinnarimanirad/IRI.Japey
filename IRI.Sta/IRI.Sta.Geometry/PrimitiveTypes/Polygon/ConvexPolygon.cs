﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.Geometry
{
    public class ConvexPolygon
    {
        //Points Are in ccw form
        public PointCollection Vertexes { get; set; }

        public List<int> Neighbours { get; set; }

        public ConvexPolygon(PointCollection vertexes)
        {
            this.Vertexes = vertexes;

            this.Neighbours = new List<int>(vertexes.Count);

            for (int i = 0; i < Neighbours.Count; i++)
            {
                Neighbours[i] = -1;
            }
        }

        public ConvexPolygon(PointCollection vertexes, List<int> neighbours)
        {
            if (vertexes.Count != neighbours.Count)
            {
                throw new NotImplementedException();
            }

            this.Vertexes = vertexes;

            this.Neighbours = neighbours;
        }

        public int Count
        {
            get { return this.Vertexes.Count; }
        }

        //private void VertexAdded()
        //{
        //    this.Neighbours.a
        //}

        public override string ToString()
        {
            if (this.Count < 0)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < Vertexes.Count - 1; i++)
            {
                result.Append(string.Format("{0}, ", Vertexes[i].ToString()));
            }

            result.Append(Vertexes[Vertexes.Count - 1].ToString());

            return result.ToString();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(QuasiPolygon))
            {
                return obj.GetHashCode() == this.GetHashCode();
            }

            return false;
        }

        public double Perimeter
        {
            get { return this.CalculatePerimeter(); }
        }

        private double CalculatePerimeter()
        {
            double tempValue = 0;

            int count = this.Count;

            for (int i = 0; i < count; i++)
            {
                int j = (i + 1) % count;

                tempValue += ComputationalGeometry.CalculateDistance(Vertexes[i], Vertexes[j]);
            }

            return tempValue;
        }

        public PointPolygonRelation GetRelationTo(Point point)
        {
            int count = this.Count;

            int tempValue = 0;

            PointVectorRelation[] relation = new PointVectorRelation[count];

            for (int i = 0; i < count; i++)
            {
                int j = (i + 1) % count;

                relation[i] = ComputationalGeometry.GetPointVectorRelation(point, Vertexes[i], Vertexes[j]);

                tempValue += (int)relation[i];

                if ((i > 0 && (int)relation[i] * (int)relation[i - 1] < 1))
                {
                    return PointPolygonRelation.Out;
                }

            }

            if (tempValue == count || tempValue == (-1 * count))
            {
                return PointPolygonRelation.In;
            }
            else
            {
                return PointPolygonRelation.On;
            }
        }

        public bool HasThePoint(Point point)
        {
            foreach (Point item in Vertexes)
            {
                if (item.Equals(point))
                {
                    return true;
                }
            }

            return false;
        }

        public double CalculateArea()
        {
            double result = 0;

            int count = Vertexes.Count;

            for (int i = 0; i < count; i++)
            {
                int j = (i + 1) % count;

                result += Vertexes[i].X * Vertexes[j].Y - Vertexes[j].X * Vertexes[i].Y;
            }

            //Polygon is counterClockWise
            if (result < 0)
            {
                throw new NotImplementedException();
            }

            return result / 2;
        }

        public List<Point> Intersect(Point firstPointLine, Point secondPointLine, out List<int> edgeIndexes)
        {
            List<Point> result = new List<Point>();

            edgeIndexes = new List<int>();

            int count = this.Vertexes.Count;

            for (int i = 0; i < count; i++)
            {
                int j = (i + 1) % count;

                Point intersection;

                LineLineSegmentRelation relation = ComputationalGeometry.IntersectLineWithLineSegment(firstPointLine, secondPointLine, Vertexes[i], Vertexes[j], out intersection);

                if (relation == LineLineSegmentRelation.Intersect)
                {
                    //check if intersection is not the vertex!
                    if (!result.Contains(intersection) && !intersection.AreTheSame(Vertexes[i], 10))
                    {
                        result.Add(intersection);

                        edgeIndexes.Add(i);
                    }
                }
                else if (relation == LineLineSegmentRelation.Coinciding)
                {
                    if (!result.Contains(Vertexes[j]))
                    {
                        result.Add(Vertexes[j]);

                        edgeIndexes.Add(i);
                    }
                }
            }

            return result;
        }
    }
}
