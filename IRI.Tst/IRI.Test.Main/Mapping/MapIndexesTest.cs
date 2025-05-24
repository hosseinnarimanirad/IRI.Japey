
using IRI.Sta.Spatial.MapIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.NetFrameworkTest.Common.Mapping;


public class MapIndexesTest
{
    /// <summary>
    /// Tests that GeodeticIndexes.Get100kIndexSheet returns the correct GeodeticExtent for sheet "5261".
    /// </summary>
    [Fact]
    public void Get100kIndexSheet_WithValidSheetCode_ReturnsExpectedGeodeticExtent()
    {
        // Arrange
        var sheetCode = "5261";
        var expectedXMin = 46;
        var expectedYMin = 35.5;

        // Act
        var sheet = GeodeticIndexes.Get100kIndexSheet(sheetCode);

        // Assert
        Assert.Equal(expectedXMin, sheet.GeodeticExtent.XMin);
        Assert.Equal(expectedYMin, sheet.GeodeticExtent.YMin);
    }
}
