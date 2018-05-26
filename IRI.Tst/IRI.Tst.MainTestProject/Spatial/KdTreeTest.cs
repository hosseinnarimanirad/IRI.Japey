using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Primitives;
using IRI.Ket.DataStructure.AdvancedStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.Spatial
{
    [TestClass]
    public class KdTreeTest
    {
        [TestMethod]
        public void TestNearestNeighbour()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(1, 9));
            points.Add(new Point(2, 2));
            points.Add(new Point(2, 5));
            points.Add(new Point(2, 10));
            points.Add(new Point(2, 12));
            points.Add(new Point(3, 7));
            points.Add(new Point(4, 11));
            points.Add(new Point(5, 8));
            points.Add(new Point(6, 7));
            points.Add(new Point(7, 3));
            points.Add(new Point(7, 4));
            points.Add(new Point(7, 5));
            points.Add(new Point(7, 11));
            points.Add(new Point(8, 3));
            points.Add(new Point(8, 4));
            points.Add(new Point(8, 5));
            points.Add(new Point(9, 3));
            points.Add(new Point(9, 4));
            points.Add(new Point(9, 5));
            points.Add(new Point(9, 10));
            points.Add(new Point(9, 11));
            points.Add(new Point(10, 3));
            points.Add(new Point(10, 10));
            points.Add(new Point(10, 11));
            points.Add(new Point(11, 6));

            Func<Point, Point, int> xWise = (p1, p2) => p1.X.CompareTo(p2.X);
            Func<Point, Point, int> yWise = (p1, p2) => p1.Y.CompareTo(p2.Y);
            Func<Point, Point, int>[] funcs = { xWise, yWise };
            var kdtree = new BalancedKdTree<Point>(points.ToArray(), funcs.ToList(), Point.NaN, i => i);

            Assert.AreEqual(new Point(6, 7), kdtree.FindNearestNeighbour(new Point(7, 7)));
            Assert.AreEqual(new Point(7, 4), kdtree.FindNearestNeighbour(new Point(7, 4)));
            Assert.AreEqual(new Point(11, 6), kdtree.FindNearestNeighbour(new Point(10, 6)));
            Assert.AreEqual(new Point(4, 11), kdtree.FindNearestNeighbour(new Point(3, 11)));

            for (int i = 0; i < 100; i++)
            {
                var point = new Point(Math.Sin(RandomHelper.Get(0, 100)) * 20,
                                        Math.Cos(RandomHelper.Get(0, 100)) * 20);

                System.Diagnostics.Debug.WriteLine(point.ToString());

                Assert.AreEqual(kdtree.FindNearestNeighbour(point), FindNearestBruteForce(points, point));
            }
        }

        [TestMethod]
        public void TestBoundingBox()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(1, 9));
            points.Add(new Point(2, 2));
            points.Add(new Point(2, 5));
            points.Add(new Point(2, 10));
            points.Add(new Point(2, 12));
            points.Add(new Point(3, 7));
            points.Add(new Point(4, 11));
            points.Add(new Point(5, 8));
            points.Add(new Point(6, 7));
            points.Add(new Point(7, 3));
            points.Add(new Point(7, 4));
            points.Add(new Point(7, 5));
            points.Add(new Point(7, 11));
            points.Add(new Point(8, 3));
            points.Add(new Point(8, 4));
            points.Add(new Point(8, 5));
            points.Add(new Point(9, 3));
            points.Add(new Point(9, 4));
            points.Add(new Point(9, 5));
            points.Add(new Point(9, 10));
            points.Add(new Point(9, 11));
            points.Add(new Point(10, 3));
            points.Add(new Point(10, 10));
            points.Add(new Point(10, 11));
            points.Add(new Point(11, 6));

            Func<Point, Point, int> xWise = (p1, p2) => p1.X.CompareTo(p2.X);
            Func<Point, Point, int> yWise = (p1, p2) => p1.Y.CompareTo(p2.Y);
            Func<Point, Point, int>[] funcs = { xWise, yWise };
            var kdtree = new BalancedKdTree<Point>(points.ToArray(), funcs.ToList(), Point.NaN, i => i);

            Assert.AreEqual(true, CheckBoundingBox(kdtree.Root));


        }



        private Point FindNearestBruteForce(List<Point> dataSet, Point targetPoint)
        {
            var minDistance = dataSet[0].DistanceTo(targetPoint);

            var result = dataSet.First();

            for (int i = 0; i < dataSet.Count; i++)
            {
                var distance = dataSet[i].DistanceTo(targetPoint);

                if (distance < minDistance)
                {
                    minDistance = distance;

                    result = dataSet[i];
                }
            }

            return result;
        }

        private bool CheckBoundingBox(BalancedKdTreeNode<Point> node)
        {
            var result = node.MinimumBoundingBox == node.CalculateBoundingBox();

            if (node.LeftChild != null && !node.LeftChild.IsNilNode())
            {
                result = result && CheckBoundingBox(node.LeftChild);
            }

            if (node.RightChild != null && !node.RightChild.IsNilNode())
            {
                result = result && CheckBoundingBox(node.RightChild);
            }

            return result;
        }

    }
}
