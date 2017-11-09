using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRI.Ham.SpatialBase;
using System.Collections.Generic;
using IRI.Ham.SpatialBase.Model;
using IRI.Ham.SpatialBase.Analysis;
using static IRI.Ham.SpatialBase.Model.SpatialRelation;

namespace IRI.Test.MainTestProject
{
    [TestClass]
    public class SpatialBaseTest
    {
        [TestMethod]
        public void TestClockwise()
        {
            List<IPoint> lineString = new List<IPoint>() { new Point(5, 0), new Point(6, 4), new Point(4, 5), new Point(1, 5), new Point(1, 0) };

            Assert.AreEqual(false, SpatialUtility.IsClockwise(lineString));

            List<IPoint> polygon = new List<IPoint>() { new Point(5, 0), new Point(6, 4), new Point(4, 1), new Point(1, 5), new Point(1, 0), new Point(5, 0) };

            Assert.AreEqual(false, SpatialUtility.IsClockwise(polygon));

            polygon.Reverse();

            Assert.AreEqual(true, SpatialUtility.IsClockwise(polygon));
        }


        [TestMethod]
        public void TestCircleRectangleIntersects()
        {
            //   -------
            //   |     |
            //   |     |
            //   |     |
            //   -------
            BoundingBox rectangle = new BoundingBox(0, 0, 10, 20);

            var radius = 5;

            Point insideRectangle = new Point(5, 9);
            Assert.AreEqual(true, SpatialUtility.CircleRectangleIntersects(insideRectangle, radius, rectangle));


            Point topOfRectangle = new Point(5, 25.1);
            Assert.AreEqual(false, SpatialUtility.CircleRectangleIntersects(topOfRectangle, radius, rectangle));


            Point bottomOfRectangle = new Point(5, -6);
            Assert.AreEqual(false, SpatialUtility.CircleRectangleIntersects(bottomOfRectangle, radius, rectangle));


            Point topRightCorner = new Point(13, 24);
            Assert.AreEqual(true, SpatialUtility.CircleRectangleIntersects(topRightCorner, radius, rectangle));

            Point topRightCorner2 = new Point(13.1, 24);
            Assert.AreEqual(false, SpatialUtility.CircleRectangleIntersects(topRightCorner2, radius, rectangle));


            Point centerRectangle = new Point(5, 10);
            Assert.AreEqual(false, SpatialUtility.IsAxisAlignedRectangleInsideCircle(centerRectangle, radius, rectangle));

            Point insideRectangle2 = new Point(5.1, 10);
            Assert.AreEqual(false, SpatialUtility.IsAxisAlignedRectangleInsideCircle(insideRectangle2, radius, rectangle));

            Point insideRectangle3 = new Point(5.1, 10);
            Assert.AreEqual(false, SpatialUtility.IsAxisAlignedRectangleInsideCircle(insideRectangle3, 11.19, rectangle));

            Point insideRectangle4 = new Point(5, 10);
            Assert.AreEqual(true, SpatialUtility.IsAxisAlignedRectangleInsideCircle(insideRectangle4, 11.19, rectangle));

            Assert.AreEqual(Intersects, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(insideRectangle, radius, rectangle));
            Assert.AreEqual(Disjoint, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(topOfRectangle, radius, rectangle));
            Assert.AreEqual(Disjoint, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(bottomOfRectangle, radius, rectangle));
            Assert.AreEqual(Intersects, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(topRightCorner, radius, rectangle));
            Assert.AreEqual(Disjoint, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(topRightCorner2, radius, rectangle));
            Assert.AreEqual(Intersects, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(centerRectangle, radius, rectangle));
            Assert.AreEqual(Intersects, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(insideRectangle2, radius, rectangle));
            Assert.AreEqual(Intersects, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(insideRectangle3, 11.19, rectangle));
            Assert.AreEqual(Contained, SpatialUtility.CalculateAxisAlignedRectangleRelationToCircle(insideRectangle4, 11.19, rectangle));

        }



    }
}
