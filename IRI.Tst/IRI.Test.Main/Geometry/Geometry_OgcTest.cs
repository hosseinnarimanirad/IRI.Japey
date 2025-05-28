using IRI.Extensions;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using Microsoft.SqlServer.Types;
using IRI.Sta.Spatial.IO.OgcSFA;
using System.Collections.Generic;
using System.Data.SqlTypes; // For SqlBytes
using Xunit;


namespace IRI.Test.Main.TheGeometry
{
    public class Geometry_OgcTest
    {
        public Geometry_OgcTest()
        {
            SqlServerTypes.Utilities.LoadNativeAssembliesv14();
        }

        public static IEnumerable<object[]> OgcGeometryTestData =>
            new List<object[]>
            {
                new object[] { "POINT(1 2)" },
                new object[] { "MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1), (9 9), (9 10), (10 9))" },
                new object[] { "MULTIPOINT((2 3), (7 8))" },
                new object[] { "LINESTRING(1 1, 2 0, 2 4, 3 3)" },
                new object[] { "LINESTRING(4 4, 9 0)" },
                new object[] { "MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))" },
                new object[] { "POLYGON((0 0, 30 0, 30 30, 0 30, 0 0))" },
                new object[] { "POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))" },
                new object[] { "POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0), (-10 0, -10 10, -15 0, -10 0))" },
                new object[] { "MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (2 1, 1 2, 1 1, 2 1)), ((9 9, 9 10, 10 9, 9 9)))" },
                new object[] { "MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (1 5, 1 1, 5 1, 5 5, 1 5)), ((4 4, 4 2, 2 2, 2 4, 4 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))" }
            };

        [Theory]
        [MemberData(nameof(OgcGeometryTestData))]
        public void TestWktToWkbToWkt_Roundtrip_UsingIriStaticFromWkb(string originalWkt)
        {
            // Arrange
            var initialGeometry = WktParser.Parse(originalWkt);
            // اگر initialGeometry اینجا null باشد یا AsWkb() مقدار null یا خالی برگرداند،
            // تست در خط بعدی یا خطوط بعد با NullReferenceException یا ArgumentNullException شکست می‌خورد.
            var wkb = initialGeometry.AsWkb();

            // Act
            var geometryFromWkb = Geometry<Point>.FromWkb(wkb, 0);
            // اگر geometryFromWkb اینجا null باشد، تست در خط بعدی با NullReferenceException شکست می‌خورد.
            var finalWkt = geometryFromWkb.AsWkt();

            // Assert
            Assert.Equal(initialGeometry.AsWkt(), finalWkt);
        }

        [Theory]
        [MemberData(nameof(OgcGeometryTestData))]
        public void TestWktToWkbToWkt_Roundtrip_UsingSqlGeometryFromWkb(string originalWkt)
        {
            // Arrange
            var initialGeometry = WktParser.Parse(originalWkt);
            var wkb = initialGeometry.AsWkb();

            // Act
            var geometryFromWkb = SqlGeometry.STGeomFromWKB(new SqlBytes(wkb), 0).AsGeometry();
            var finalWkt = geometryFromWkb.AsWkt();

            // Assert
            Assert.Equal(initialGeometry.AsWkt(), finalWkt);
        }

        public static IEnumerable<object[]> WktSerializationTestData()
        {
            foreach (var itemArray in OgcGeometryTestData)
            {
                var inputWkt = (string)itemArray[0];
                var expectedWkt = inputWkt.Contains("POINT") && !inputWkt.Contains("MULTIPOINT") ? inputWkt : inputWkt.Replace(", ", ",");
                yield return new object[] { inputWkt, expectedWkt };
            }
        }

        [Theory]
        [MemberData(nameof(WktSerializationTestData))]
        public void TestWkt_SerializationAndParsing_Roundtrip(string inputWkt, string expectedCanonicalWkt)
        {
            // Arrange
            var parsedGeometry = WktParser.Parse(inputWkt);

            // Act
            var serializedWkt = parsedGeometry.AsWkt();

            // Assert
            Assert.Equal(expectedCanonicalWkt, serializedWkt);

            var reparsedGeometry = WktParser.Parse(serializedWkt);
            // اگر reparsedGeometry اینجا null باشد، تست در خط بعدی با NullReferenceException شکست می‌خورد.
            Assert.Equal(expectedCanonicalWkt, reparsedGeometry.AsWkt());
        }
    }
}