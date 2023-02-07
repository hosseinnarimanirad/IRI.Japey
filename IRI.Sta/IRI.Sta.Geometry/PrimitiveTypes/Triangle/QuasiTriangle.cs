// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Msh.Common.Analysis.Topology;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.Geometry
{
    public class QuasiTriangle
    {
        private int first, second, third;

        private readonly int code;

        private int firstSecondNeighbour, secondThirdNeighbour, thirdFirstNeighbour;

        //directed arrow from first to second edge
        public static readonly int firstEdgeWeight = 1;

        //directed arrow from second to third edge
        public static readonly int secondEdgeWeight = 3;

        //directed arrow from third to first edge
        public static readonly int thirdEdgeWeight = 9;

        public int First
        {
            get { return first; }
            set { first = value; }
        }

        public int Second
        {
            get { return second; }
            set { second = value; }
        }

        public int Third
        {
            get { return third; }
            set { third = value; }
        }

        public int FirstSecondNeighbour
        {
            get { return this.firstSecondNeighbour; }
            set { this.firstSecondNeighbour = value; }
        }

        public int SecondThirdNeighbour
        {
            get { return this.secondThirdNeighbour; }
            set { this.secondThirdNeighbour = value; }
        }

        public int ThirdFirstNeighbour
        {
            get { return this.thirdFirstNeighbour; }
            set { this.thirdFirstNeighbour = value; }
        }

        public int[] OrderedNeighbours
        {
            get { return new int[] { this.FirstSecondNeighbour, this.SecondThirdNeighbour, this.ThirdFirstNeighbour }; }
        }

        public QuasiTriangle()
        {
            this.firstSecondNeighbour = -1;

            this.secondThirdNeighbour = -1;

            this.thirdFirstNeighbour = -1;
        }

        public QuasiTriangle(int first, int second, int third, int code)
        {
            this.code = code;

            this.first = first;

            this.second = second;

            this.third = third;

            this.firstSecondNeighbour = -1;

            this.secondThirdNeighbour = -1;

            this.thirdFirstNeighbour = -1;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", first, second, third);
        }

        public override int GetHashCode()
        {
            return this.code;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(QuasiTriangle))
            {
                QuasiTriangle tempTriangle = (QuasiTriangle)obj;

                return tempTriangle.First == this.First &&
                        tempTriangle.Second == this.Second &&
                        tempTriangle.Third == this.Third;
            }

            return false;
        }

        public bool HasThePoint(int pointCode)
        {
            return this.first == pointCode ||
                    this.second == pointCode ||
                    this.third == pointCode;
        }

        public TriangleRelation GetRelationTo(QuasiTriangle triangle)
        {
            int hasTheFirst = System.Convert.ToInt32(triangle.HasThePoint(this.First));

            int hasTheSecond = System.Convert.ToInt32(triangle.HasThePoint(this.Second));

            int hasTheThird = System.Convert.ToInt32(triangle.HasThePoint(this.Third));

            return (TriangleRelation)((hasTheFirst * (int)TriangleRelation.MeetAtFirst) +
                                        hasTheSecond * (int)TriangleRelation.MeetAtSecond +
                                        hasTheThird * (int)TriangleRelation.MeetAtThird);
        }

        public bool HasTheEdge(QuasiEdge edge)
        {
            return (this.HasThePoint(edge.First) && this.HasThePoint(edge.Second));
        }

        public int GetThirdPoint(QuasiEdge edge)
        {
            if (!HasTheEdge(edge))
            {
                throw new NotImplementedException();
            }

            if (this.first != edge.First && this.first != edge.Second)
            {
                return this.first;
            }
            else if (this.Second != edge.First && this.Second != edge.Second)
            {
                return this.Second;
            }
            else if (this.third != edge.First && this.third != edge.Second)
            {
                return this.third;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public int GetNeighbour(QuasiEdge commonEdge)
        {
            if (!HasTheEdge(commonEdge))
            {
                throw new NotImplementedException();
            }

            if (this.first == commonEdge.First)
            {
                if (this.second == commonEdge.Second)
                {
                    return this.FirstSecondNeighbour;
                }
                else if (this.third == commonEdge.Second)
                {
                    return this.ThirdFirstNeighbour;
                }
            }
            else if (this.second == commonEdge.First)
            {
                if (this.first == commonEdge.Second)
                {
                    return this.FirstSecondNeighbour;
                }
                else if (this.third == commonEdge.Second)
                {
                    return this.SecondThirdNeighbour;
                }
            }
            else if (this.third == commonEdge.First)
            {
                if (this.first == commonEdge.Second)
                {
                    return this.ThirdFirstNeighbour;
                }
                else if (this.second == commonEdge.Second)
                {
                    return this.SecondThirdNeighbour;
                }
            }

            throw new NotImplementedException();
        }


    }
}