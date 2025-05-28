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

        // Data source based on variables in original TestGeometryToWkb()
        public static IEnumerable<object[]> WktTestDataFromOriginalFact =>
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
                new object[] { "MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((2 4, 2 2, 4 2, 4 4, 2 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))" }
            };

        // Data source based on InlineData in original TestWkbToGeometry()
        public static IEnumerable<object[]> WktTestDataFromOriginalTheory =>
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
        [MemberData(nameof(WktTestDataFromOriginalFact))] // Uses data from the original Fact's variables
        public void TestWkbRoundtrip_ViaSqlGeometry(string originalWkt)
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

        [Theory]
        [MemberData(nameof(WktTestDataFromOriginalTheory))] // Uses data from the original Theory's InlineData
        public void TestWkbRoundtrip_ViaIriStaticMethod(string originalWkt)
        {
            // Arrange
            var initialGeometry = WktParser.Parse(originalWkt);
            var wkb = initialGeometry.AsWkb();

            // Act
            var geometryFromWkb = Geometry<Point>.FromWkb(wkb, 0);
            var finalWkt = geometryFromWkb.AsWkt();

            // Assert
            Assert.Equal(initialGeometry.AsWkt(), finalWkt);
        }

        // Data source for WKT serialization, based on string variables in original TestWktToGeometry()
        public static IEnumerable<object[]> WktSerializationTestData()
        {
            // These WKT strings are identical to WktTestDataFromOriginalFact in this case
            var wktStrings = new List<string>
            {
                "POINT(1 2)",
                "MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1), (9 9), (9 10), (10 9))",
                "MULTIPOINT((2 3), (7 8))",
                "LINESTRING(1 1, 2 0, 2 4, 3 3)",
                "LINESTRING(4 4, 9 0)",
                "MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))",
                "POLYGON((0 0, 30 0, 30 30, 0 30, 0 0))",
                "POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))",
                "POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0), (-10 0, -10 10, -15 0, -10 0))",
                "MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (2 1, 1 2, 1 1, 2 1)), ((9 9, 9 10, 10 9, 9 9)))",
                "MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((2 4, 2 2, 4 2, 4 4, 2 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))"
            };

            foreach (var inputWkt in wktStrings)
            {
                // Logic from original TestWktToGeometry for expected output
                var expectedWkt = (inputWkt.StartsWith("POINT(") && !inputWkt.StartsWith("MULTIPOINT("))
                                    ? inputWkt
                                    : inputWkt.Replace(", ", ",");
                yield return new object[] { inputWkt, expectedWkt };
            }
        }

        [Theory]
        [MemberData(nameof(WktSerializationTestData))]
        public void TestWktSerialization_ParseThenAsWkt(string inputWkt, string expectedSerializedWkt)
        {
            // Arrange
            var parsedGeometry = WktParser.Parse(inputWkt);

            // Act
            var actualSerializedWkt = parsedGeometry.AsWkt();

            // Assert
            Assert.Equal(expectedSerializedWkt, actualSerializedWkt);
        }
    }
}