using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRI.Sta.CoordinateSystem;
using IRI.Sta.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Mapping;

namespace IRI.Test.MainTestProject
{
    [TestClass]
    public class WebMercatorUtilityTest
    {
        [TestMethod]
        public void TestGeodeticToGoogleRowColumn()
        {
            var rowColumn01 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 60, geocentricLongitude: -140, zoom: 3);
            Assert.AreEqual(0, rowColumn01.X);
            Assert.AreEqual(2, rowColumn01.Y);

            var rowColumn02 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -60, geocentricLongitude: -140, zoom: 3);
            Assert.AreEqual(0, rowColumn02.X);
            Assert.AreEqual(5, rowColumn02.Y);

            var rowColumn03 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -84, geocentricLongitude: 40, zoom: 3);
            Assert.AreEqual(4, rowColumn03.X);
            Assert.AreEqual(7, rowColumn03.Y);

            var rowColumn04 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 31.1, geocentricLongitude: 42.5, zoom: 8);
            Assert.AreEqual(158, rowColumn04.X);
            Assert.AreEqual(104, rowColumn04.Y);

            var rowColumn05 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 84.6, geocentricLongitude: -179.3, zoom: 6);
            Assert.AreEqual(0, rowColumn05.X);
            Assert.AreEqual(0, rowColumn05.Y);

            var rowColumn06 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 4.6, geocentricLongitude: -170.3, zoom: 6);
            Assert.AreEqual(1, rowColumn06.X);
            Assert.AreEqual(31, rowColumn06.Y);

            var rowColumn07 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -4.3, geocentricLongitude: -170.3, zoom: 6);
            Assert.AreEqual(1, rowColumn07.X);
            Assert.AreEqual(32, rowColumn07.Y);

            var rowColumn08 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -79.5, geocentricLongitude: -2, zoom: 6);
            Assert.AreEqual(31, rowColumn08.X);
            Assert.AreEqual(56, rowColumn08.Y);

            var rowColumn09 = IRI.Msh.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 22.5, geocentricLongitude: 2, zoom: 6);
            Assert.AreEqual(32, rowColumn09.X);
            Assert.AreEqual(27, rowColumn09.Y);


        }

        [TestMethod]
        public void TestGoogleImageNumberToBoundingBox()
        {
            var boundingBox01 = IRI.Msh.Common.Mapping.WebMercatorUtility.GetWgs84ImageBoundingBox(6, 0, 3);

            Assert.AreEqual(-180, boundingBox01.XMin);
            Assert.AreEqual(-135, boundingBox01.XMax);
            Assert.AreEqual(-66.51326043946698, boundingBox01.YMax, 1E-12);
            Assert.AreEqual(-79.17133463824213, boundingBox01.YMin, 1E-12);

            //-180     -  79.17133464081945
            //- 135     - 66.51326044311185

            var boundingBox02 = IRI.Msh.Common.Mapping.WebMercatorUtility.GetWgs84ImageBoundingBox(1, 5, 3);

            Assert.AreEqual(45, boundingBox02.XMin);
            Assert.AreEqual(90, boundingBox02.XMax);
            Assert.AreEqual(79.17133463824213, boundingBox02.YMax, 1E-12);
            Assert.AreEqual(66.51326043946698, boundingBox02.YMin, 1E-12);




            var boundingBox03 = IRI.Msh.Common.Mapping.WebMercatorUtility.GetWgs84ImageBoundingBox(100, 164, 8);

            //WGS84 Actual bounding box based on http://www.maptiler.org/google-maps-coordinates-tile-bounds-projection/
            //50.625        35.460669951495305
            //52.03125      36.59788913307022

            Assert.AreEqual(50.625, boundingBox03.XMin);
            Assert.AreEqual(52.03125, boundingBox03.XMax);
            Assert.AreEqual(36.59788913307022, boundingBox03.YMax, 1E-8);
            Assert.AreEqual(35.460669951495305, boundingBox03.YMin, 1E-8);


            //5635549.221409474 4226661.916057106
            //5792092.255337514 4383204.9499851465

            var geocentricBL = MapProjects.GeodeticToMercator(new IRI.Msh.Common.Primitives.Point(boundingBox03.XMin, boundingBox03.YMin), Ellipsoids.Sphere);

        }

        [TestMethod]
        public void TestCalculateMapScale()
        {
            Assert.AreEqual(1.0 / 591658710.91, WebMercatorUtility.CalculateMapScale(0, 00.0), 0.1);
            Assert.AreEqual(1.0 / 153132542.58, WebMercatorUtility.CalculateMapScale(0, 75.0), 0.1);

            Assert.AreEqual(1.0 / 18489334.72, WebMercatorUtility.CalculateMapScale(5, 00.0), 0.1);
            Assert.AreEqual(1.0 / 4785391.96, WebMercatorUtility.CalculateMapScale(5, 75.0), 0.1);

            Assert.AreEqual(1.0 / 72223.96, WebMercatorUtility.CalculateMapScale(13, 00.0), 0.1);
            Assert.AreEqual(1.0 / 18692.94, WebMercatorUtility.CalculateMapScale(13, 75.0), 0.1);

            Assert.AreEqual(1.0 / 141.06, WebMercatorUtility.CalculateMapScale(22, 00.0), 0.1);
            Assert.AreEqual(1.0 / 036.51, WebMercatorUtility.CalculateMapScale(22, 75.0), 0.1);

        }

        [TestMethod]
        public void TestGetZoomLevelMethods()
        {
            for (int level = 1; level < 23; level++)
            {
                for (int latitude = 0; latitude < 75; latitude += 5)
                {
                    var groundDistance = WebMercatorUtility.CalculateGroundResolution(level, latitude);

                    var calculatedLevel = WebMercatorUtility.GetZoomLevel(groundDistance * 256, latitude, 256);

                    Assert.AreEqual(level, calculatedLevel);
                }
            }
        }

        [TestMethod]
        public void TestGetUpperAndLowerZoomLevels()
        {

            Assert.AreEqual(1, WebMercatorUtility.GetUpperLevel(1.0 / 150000000, 30).ZoomLevel);
            Assert.AreEqual(1, WebMercatorUtility.GetUpperLevel(1.0 / 140000000, 30).ZoomLevel);

            Assert.AreEqual(2, WebMercatorUtility.GetLowerLevel(1.0 / 150000000, 30).ZoomLevel);
            Assert.AreEqual(2, WebMercatorUtility.GetLowerLevel(1.0 / 140000000, 30).ZoomLevel);

        }


        //[TestMethod]
        //public void TestGoogleImageNumberTo
    }
}
