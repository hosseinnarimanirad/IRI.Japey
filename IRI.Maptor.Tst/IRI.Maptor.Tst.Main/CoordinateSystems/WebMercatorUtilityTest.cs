using IRI.Maptor.Sta.Spatial.Helpers;
using IRI.Maptor.Sta.SpatialReferenceSystem;

namespace IRI.Maptor.Tst.CoordinateSystems;

public class WebMercatorUtilityTest
{
    [Fact]
    public void TestLatLongToImageNumber()
    {
        var imageNumber = WebMercatorUtility.LatLonToImageNumber(66.5, 89.9999, 2);
        Assert.Equal(2, imageNumber.X);
        Assert.Equal(1, imageNumber.Y);


        var imageNumber2 = WebMercatorUtility.LatLonToImageNumber(0.01, -45.1, 3);
        Assert.Equal(2, imageNumber2.X);
        Assert.Equal(3, imageNumber2.Y);


        var imageNumber3 = WebMercatorUtility.LatLonToImageNumber(-31.953, 22.501, 7);
        Assert.Equal(72, imageNumber3.X);
        Assert.Equal(76, imageNumber3.Y);


        var imageNumber4 = WebMercatorUtility.LatLonToImageNumber(27.197, 60.678, 15);
        Assert.Equal(21907, imageNumber4.X);
        Assert.Equal(13809, imageNumber4.Y);
    }

    [Fact]
    public void TestGeodeticToGoogleRowColumn()
    {
        var rowColumn01 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 60, geocentricLongitude: -140, zoom: 3);
        Assert.Equal(0, rowColumn01.X);
        Assert.Equal(2, rowColumn01.Y);

        var rowColumn02 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -60, geocentricLongitude: -140, zoom: 3);
        Assert.Equal(0, rowColumn02.X);
        Assert.Equal(5, rowColumn02.Y);

        var rowColumn03 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -84, geocentricLongitude: 40, zoom: 3);
        Assert.Equal(4, rowColumn03.X);
        Assert.Equal(7, rowColumn03.Y);

        var rowColumn04 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 31.1, geocentricLongitude: 42.5, zoom: 8);
        Assert.Equal(158, rowColumn04.X);
        Assert.Equal(104, rowColumn04.Y);

        var rowColumn05 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 84.6, geocentricLongitude: -179.3, zoom: 6);
        Assert.Equal(0, rowColumn05.X);
        Assert.Equal(0, rowColumn05.Y);

        var rowColumn06 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 4.6, geocentricLongitude: -170.3, zoom: 6);
        Assert.Equal(1, rowColumn06.X);
        Assert.Equal(31, rowColumn06.Y);

        var rowColumn07 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -4.3, geocentricLongitude: -170.3, zoom: 6);
        Assert.Equal(1, rowColumn07.X);
        Assert.Equal(32, rowColumn07.Y);

        var rowColumn08 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: -79.5, geocentricLongitude: -2, zoom: 6);
        Assert.Equal(31, rowColumn08.X);
        Assert.Equal(56, rowColumn08.Y);

        var rowColumn09 = WebMercatorUtility.LatLonToImageNumber(geocentricLatitude: 22.5, geocentricLongitude: 2, zoom: 6);
        Assert.Equal(32, rowColumn09.X);
        Assert.Equal(27, rowColumn09.Y);


    }

    [Fact]
    public void TestGoogleImageNumberToBoundingBox()
    {
        var boundingBox01 = WebMercatorUtility.GetWgs84ImageBoundingBox(6, 0, 3);

        Assert.Equal(-180, boundingBox01.XMin);
        Assert.Equal(-135, boundingBox01.XMax);
        Assert.Equal(-66.51326043946698, boundingBox01.YMax, 12/*1E-12*/);
        Assert.Equal(-79.17133463824213, boundingBox01.YMin, 12);

        //-180     -  79.17133464081945
        //- 135     - 66.51326044311185

        var boundingBox02 = WebMercatorUtility.GetWgs84ImageBoundingBox(1, 5, 3);

        Assert.Equal(45, boundingBox02.XMin);
        Assert.Equal(90, boundingBox02.XMax);
        Assert.Equal(79.17133463824213, boundingBox02.YMax, 12/*1E-12*/);
        Assert.Equal(66.51326043946698, boundingBox02.YMin, 12);




        var boundingBox03 = WebMercatorUtility.GetWgs84ImageBoundingBox(100, 164, 8);

        //WGS84 Actual bounding box based on http://www.maptiler.org/google-maps-coordinates-tile-bounds-projection/
        //50.625        35.460669951495305
        //52.03125      36.59788913307022

        Assert.Equal(50.625, boundingBox03.XMin);
        Assert.Equal(52.03125, boundingBox03.XMax);
        Assert.Equal(36.59788913307022, boundingBox03.YMax, 8/*1E-8*/);
        Assert.Equal(35.460669951495305, boundingBox03.YMin, 8/*1E-8*/);


        //5635549.221409474 4226661.916057106
        //5792092.255337514 4383204.9499851465

        var geocentricBL = MapProjects.GeodeticToMercator(new IRI.Maptor.Sta.Common.Primitives.Point(boundingBox03.XMin, boundingBox03.YMin), Ellipsoids.Sphere);

    }

    [Fact]
    public void TestCalculateMapScale()
    {
        Assert.Equal(1.0 / 591658710.91, WebMercatorUtility.CalculateMapScale(0, 00.0), 2/*0.1*/);
        Assert.Equal(1.0 / 153132542.58, WebMercatorUtility.CalculateMapScale(0, 75.0), 2/*0.1*/);

        Assert.Equal(1.0 / 18489334.72, WebMercatorUtility.CalculateMapScale(5, 00.0), 2 /*0.1*/);
        Assert.Equal(1.0 / 4785391.96, WebMercatorUtility.CalculateMapScale(5, 75.0), 2 /*0.1*/);

        Assert.Equal(1.0 / 72223.96, WebMercatorUtility.CalculateMapScale(13, 00.0), 2 /*0.1*/);
        Assert.Equal(1.0 / 18692.94, WebMercatorUtility.CalculateMapScale(13, 75.0), 2 /*0.1*/);

        Assert.Equal(1.0 / 141.06, WebMercatorUtility.CalculateMapScale(22, 00.0), 2 /*0.1*/);
        Assert.Equal(1.0 / 036.51, WebMercatorUtility.CalculateMapScale(22, 75.0), 2 /*0.1*/);

    }

    [Fact]
    public void TestGetZoomLevelMethods()
    {
        for (int level = 1; level < 23; level++)
        {
            //for (int latitude = 0; latitude < 75; latitude += 5)
            //{
            var groundDistance = WebMercatorUtility.ToWebMercatorLength(level, 256);

            var calculatedLevel = WebMercatorUtility.EstimateZoomLevel(groundDistance /** 256*/, /*latitude,*/ 256);

            Assert.Equal(level, calculatedLevel);
            //}
        }
    }

    [Fact]
    public void TestGetUpperAndLowerZoomLevels()
    {

        Assert.Equal(1, WebMercatorUtility.GetUpperLevel(1.0 / 150000000, 30).ZoomLevel);
        Assert.Equal(1, WebMercatorUtility.GetUpperLevel(1.0 / 140000000, 30).ZoomLevel);

        Assert.Equal(2, WebMercatorUtility.GetLowerLevel(1.0 / 150000000, 30).ZoomLevel);
        Assert.Equal(2, WebMercatorUtility.GetLowerLevel(1.0 / 140000000, 30).ZoomLevel);

    }


    //[Fact]
    //public void TestGoogleImageNumberTo
}
