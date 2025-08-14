
//namespace IRI.Maptor.Tst.Esri;
 
//public class EsriShape_KmlTest
//{
//    [Fact]
//    public void TestParseToString()
//    {
//        string shpFileName = @"E:\Data\0. UnitTestData\Geographic Shapefiles\ArcMap Output\Point.shp";

//        var shapes = IRI.Maptor.Sta.ShapefileFormat.Shapefile.ReadShapes(shpFileName);

//        var IShape = shapes[0];

//        string value = IRI.Maptor.Sta.Common.Helpers.XmlHelper.Parse(IShape.AsPlacemark());
//    }

//    [Fact]
//    public void TestAsKmlForPoint()
//    {
//        string shpFileName = @"E:\Data\0. UnitTestData\Geographic Shapefiles\ArcMap Output\Point.shp";

//        string kmlFileName = @"E:\Data\0. UnitTestData\Geographic Shapefiles\My Output\Point.kml";

//        string computedKml = IRI.Maptor.Sta.ShapefileFormat.Shapefile.ReadShapes(shpFileName).AsKml();

//        string actualKml = System.IO.File.ReadAllText(kmlFileName);

//        Assert.Equal(computedKml, actualKml);
//    }

//    [Fact]
//    public void TestAsKmlForLineString()
//    {
//        string shpFileName = @"E:\Data\0. UnitTestData\Geographic Shapefiles\ArcMap Output\MultipartLinestring.shp";

//        string kmlFileName = @"E:\Data\0. UnitTestData\Geographic Shapefiles\My Output\MultipartLinestring.kml";

//        string computedKml = IRI.Maptor.Sta.ShapefileFormat.Shapefile.ReadShapes(shpFileName).AsKml();

//        string actualKml = System.IO.File.ReadAllText(kmlFileName);

//        Assert.Equal(computedKml, actualKml);
//    }

//}
