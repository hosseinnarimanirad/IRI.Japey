using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Spatial.DataStructures
{
    public class SFCRTree
    {
        public RTreeNode Root;

        private Func<Rectangle, Rectangle,  Primitives.Boundary, int> comparer;

        public static int HilbertComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertComparer(firstPoint, secondPoint, boundary);
        }

        public static int NOrderingComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.NOrderingComparer(firstPoint, secondPoint, boundary);
        }

        public static int GrayComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.GrayComparer(firstPoint, secondPoint, boundary);
        }

        public static int ZOrderingComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.ZOrderingComparer(firstPoint, secondPoint, boundary);
        }

        public static int DiagonalLebesgueComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.DiagonalLebesgueComparer(firstPoint, secondPoint, boundary);
        }

        public static int UOrderOrLebesgueSquareComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.UOrderOrLebesgueSquareComparer(firstPoint, secondPoint, boundary);
        }

        public static int PeanoComparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.PeanoComparer(firstPoint, secondPoint, boundary);
        }

        public static int Peano02Comparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.Peano02Comparer(firstPoint, secondPoint, boundary);
        }

        public static int Peano03Comparer(Rectangle first, Rectangle second,  Primitives.Boundary boundary)
        {
            Point firstPoint = new  Point(first.CenterX, first.CenterY);

            Point secondPoint = new  Point(second.CenterX, second.CenterY);

            return IRI.Ket.Spatial.PointSorting.PointOrdering.Peano03Comparer(firstPoint, secondPoint, boundary);
        }


        private SFCRTree(Rectangle[] values, int minimumDegree)
            : this(values, SFCRTree.HilbertComparer, minimumDegree) { }

        public SFCRTree(Rectangle[] values, Func<Rectangle, Rectangle,  Primitives.Boundary, int> comparer, int minimumDegree)
        {
            this.Root = new RTreeNode();

            this.minimumDegree = minimumDegree;

            this.comparer = comparer;

            this.Root.IsLeaf = true;

            foreach (Rectangle item in values)
            {
                Insert(item);
            }
        }

        int minimumDegree;

        public int MinimumDegree
        {
            get { return this.minimumDegree; }
        }

        private  Primitives.Boundary boundary;

        public  Primitives.Boundary Boundary
        {
            get
            {
                return this.boundary;
            }
        }

        //Untested
        public void Insert(Rectangle key)
        {
            //
            Rectangle newBoundary = this.Root.Boundary.Add(key);

             Point lowerLeft =
                  new  Point(newBoundary.minX, newBoundary.minY);

             Point upperRight =
                new  Point(newBoundary.maxX, newBoundary.maxY);

            this.boundary = new  Primitives.Boundary(lowerLeft, upperRight);
            //

            RTreeNode r = this.Root;

            if (r.NumberOfKeys == 2 * this.MinimumDegree - 1)
            {
                RTreeNode s = new RTreeNode();

                this.Root = s;

                s.IsLeaf = false;

                s.AddPointer(r);

                SplitTheNode(s, 0);

                InsertNonFull(s, key);
            }
            else
            {
                InsertNonFull(r, key);
            }
        }

        //Untested
        public void SplitTheNode(RTreeNode parent, int childIndex)
        {
            RTreeNode y = parent.GetPointer(childIndex);

            int t = this.MinimumDegree;

            RTreeNode z = new RTreeNode();

            z.IsLeaf = y.IsLeaf;

            //z.AddRangeKeys(y.SkipKeys(t));

            if (y.IsLeaf)
            {
                z.AddRangeKeys(y.SkipKeys(t));

                //z.AddRangePointers(y.SkipPointers(t));

                y.RemoveRangeKeys(t, t - 1);
            }
            else
            {
                z.AddRangePointers(y.SkipPointers(t));

                y.RemoveRangePointers(t, t - 1);
            }

            parent.InsertPointer(childIndex + 1, z);
        }

        //Untested
        public void InsertNonFull(RTreeNode x, Rectangle k)
        {
            int i = x.NumberOfKeys - 1;

            if (x.IsLeaf)
            {
                while (i > 0 && comparer(k, x.GetKey(i), this.Boundary) < 0)
                {
                    i--;
                }

                x.InsertKey(i + 1, k);
            }
            else
            {
                while (i > 0 && comparer(k, x.GetPointer(i).Boundary, this.Boundary) < 0)
                {
                    i--;
                }

                if (x.GetPointer(i).NumberOfKeys == 2 * this.MinimumDegree - 1)
                {
                    SplitTheNode(x, i);

                    if (comparer(k, x.GetPointer(i).Boundary, this.Boundary) > 0)
                    {
                        i++;
                    }
                }

                InsertNonFull(x.GetPointer(i), k);

                x.FixBoundary();
            }
        }
    }
}