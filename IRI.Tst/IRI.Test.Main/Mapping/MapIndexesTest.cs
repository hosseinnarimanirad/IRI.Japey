
using IRI.Sta.Spatial.MapIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.NetFrameworkTest.Common.Mapping;


public class MapIndexesTest
{
    [Fact]
    public void TestMapIndexes()
    {
        var sheet = GeodeticIndexes.Get100kIndexSheet("5261");

        Assert.Equal(46, sheet.GeodeticExtent.XMin);
        Assert.Equal(35.5, sheet.GeodeticExtent.YMin);            
    }
}
