
using IRI.Sta.Spatial.MapIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.NetFrameworkTest.Common.Mapping
{
    public class MapIndexesTest
    {
        [Theory]
        [InlineData("5261", 46.0, 35.5, 46.5, 36.0)]
        [InlineData("5372", 46.5, 36.0, 47.0, 36.5)]
        [InlineData("5483", 47.0, 36.5, 47.5, 37.0)]
        public void Get100kGeodeticExtent_ValidIndex_ReturnsCorrectExtent(string index100k, double expectedXMin, double expectedYMin, double expectedXMax, double expectedYMax)
        {
            // Act
            var sheet = GeodeticIndexes.Get100kIndexSheet(index100k);
            Assert.NotNull(sheet);

            var extent = sheet.GeodeticExtent;
            Assert.True(extent.HasValue, "GeodeticExtent is null");

            // Assert
            Assert.Equal(expectedXMin, extent!.Value.XMin);
            Assert.Equal(expectedYMin, extent.Value.YMin);
            Assert.Equal(expectedXMax, extent.Value.XMax);
            Assert.Equal(expectedYMax, extent.Value.YMax);
        }
    }
    }
