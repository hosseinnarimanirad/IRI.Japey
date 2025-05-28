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
            // This loads native assemblies for SQL Server spatial types.
            // It's fine in the constructor if it's safe to call multiple times
            // or if xUnit creates a new class instance for each test method (default behavior).
            SqlServerTypes.Utilities.LoadNativeAssembliesv14();
        }

        // Centralized test data for WKT strings
        // These WKTs are taken from your first Fact test (TestGeometryToWkb)
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
                new object[] { "MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((2 4, 2 2, 4 2, 4 4, 2 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))" }
            };

        /// <summary>
        /// Helper method to convert WKB bytes back to an IRI Geometry object
        /// using SqlGeometry.STGeomFromWKB (as per your original private FromWkb helper).
        /// </summary>
        private Geometry<Point> ParseWkbUsingSqlGeometry(byte[] wkbData, int srid = 0)
        {
            // This wraps the SqlServerTypes WKB parsing.
            return SqlGeometry.STGeomFromWKB(new SqlBytes(wkbData), srid).AsGeometry();
        }

        /// <summary>
        /// Helper method to convert WKB bytes back to an IRI Geometry object
        /// using the IRI library's static FromWkb method (as per your original Theory test).
        /// </summary>
        private Geometry<Point> ParseWkbUsingIriStaticMethod(byte[] wkbData, int srid = 0)
        {
            // This uses your library's WKB parsing.
            // Assuming Geometry<Point>.FromWkb is the correct static method.
            return Geometry<Point>.FromWkb(wkbData, srid);
        }

        // --- Test Section for WKB Roundtrip ---

        [Theory]
        [MemberData(nameof(OgcGeometryTestData))]
        public void TestWktToWkbToWkt_Roundtrip_UsingIriStaticFromWkb(string originalWkt)
        {
            // Arrange
            var initialGeometry = WktParser.Parse(originalWkt);
            Assert.NotNull(initialGeometry); // Ensure parsing the input WKT was successful

            // Act
            var wkb = initialGeometry.AsWkb(); // Geometry -> WKB
            Assert.NotNull(wkb);
            Assert.NotEmpty(wkb);

            // WKB -> Geometry using your library's static method
            var geometryFromWkb = ParseWkbUsingIriStaticMethod(wkb, 0);
            Assert.NotNull(geometryFromWkb);

            var finalWkt = geometryFromWkb.AsWkt(); // Geometry -> WKT

            // Assert
            // Verifies that WKT -> Geometry -> WKB -> Geometry (IRI) -> WKT results in the original WKT (or its canonical form)
            Assert.Equal(initialGeometry.AsWkt(), finalWkt);
        }

        [Theory]
        [MemberData(nameof(OgcGeometryTestData))]
        public void TestWktToWkbToWkt_Roundtrip_UsingSqlGeometryFromWkb(string originalWkt)
        {
            // Arrange
            var initialGeometry = WktParser.Parse(originalWkt);
            Assert.NotNull(initialGeometry);

            // Act
            var wkb = initialGeometry.AsWkb(); // Geometry -> WKB
            Assert.NotNull(wkb);
            Assert.NotEmpty(wkb);

            // WKB -> Geometry using SqlGeometry
            var geometryFromWkb = ParseWkbUsingSqlGeometry(wkb, 0);
            Assert.NotNull(geometryFromWkb);

            var finalWkt = geometryFromWkb.AsWkt(); // Geometry -> WKT

            // Assert
            // Verifies that WKT -> Geometry -> WKB -> Geometry (Sql) -> WKT results in the original WKT (or its canonical form)
            Assert.Equal(initialGeometry.AsWkt(), finalWkt);
        }


        // --- Test Section for WKT Serialization and Parsing ---

        /// <summary>
        /// Provides test data for WKT parsing and serialization,
        /// including the input WKT and the expected normalized WKT output from AsWkt().
        /// The expected WKT is the input WKT with no space after commas.
        /// </summary>
        public static IEnumerable<object[]> WktSerializationTestData()
        {
            foreach (var itemArray in OgcGeometryTestData)
            {
                var inputWkt = (string)itemArray[0];
                // Assuming AsWkt() normalizes by removing space after comma, as suggested by your original test.
                // If AsWkt() produces exactly the input WKT from OgcGeometryTestData, then no Replace is needed.
                // This matches the logic from your original TestWktToGeometry (Fact)
                var expectedWkt = inputWkt.Contains("POINT") && !inputWkt.Contains("MULTIPOINT") ? inputWkt : inputWkt.Replace(", ", ",");
                yield return new object[] { inputWkt, expectedWkt };
            }
        }

        [Theory]
        [MemberData(nameof(WktSerializationTestData))]
        public void TestWkt_SerializationAndParsing_Roundtrip(string inputWkt, string expectedCanonicalWkt)
        {
            // Arrange
            // inputWkt and expectedCanonicalWkt are provided by MemberData

            // Act
            // 1. Parse the input WKT string to a geometry object.
            var parsedGeometry = WktParser.Parse(inputWkt);
            Assert.NotNull(parsedGeometry);

            // 2. Serialize the geometry object back to its WKT string representation.
            var serializedWkt = parsedGeometry.AsWkt();

            // Assert
            // 3. Verify that the serialized WKT matches the expected canonical WKT.
            Assert.Equal(expectedCanonicalWkt, serializedWkt);

            // Optional: Further test that parsing the serializedWkt yields an equivalent geometry
            // This ensures that your AsWkt() output is valid and parseable back to the same geometry.
            var reparsedGeometry = WktParser.Parse(serializedWkt);
            Assert.NotNull(reparsedGeometry);
            // This assumes your geometry objects have a meaningful equality check or you compare their WKTs again.
            Assert.Equal(expectedCanonicalWkt, reparsedGeometry.AsWkt());
        }
    }
}