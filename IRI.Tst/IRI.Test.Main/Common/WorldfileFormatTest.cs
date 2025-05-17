using IRI.Ket.GdiPlus.WorldfileFormat;
using IRI.Sta.Spatial.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.NetFrameworkTest.Common;


public class WorldfileFormatTest
{
    string vanakUtm = $"Assets\\WorldfileSamples\\vanakUTM.tfw";
    int vanakHeight = 2095;
    int vanakWidth = 2881;


    string Ni = $"Assets\\WorldfileSamples\\NI38.8.jpgw";
    int niHeight = 3017;
    int niWidth = 3784;

    [Fact]
    public void TestWorldfile()
    {
        Test(vanakUtm, vanakWidth, vanakHeight);

        Test(Ni, niWidth, niHeight);
        //var vanakBbx1 = WorldfileManager.ReadImageBoundingBox(vanakUtm, vanakHeight, vanakWidth);

        //var vanakWorldfile = Worldfile.Read(vanakUtm);

        //var vanakBbx2 = vanakWorldfile.GetBoundingBox(vanakWidth, vanakHeight);

        //Assert.Equal(vanakBbx1, vanakBbx2);

        //var calculatedVanakWorldfile = WorldfileManager.Create(vanakBbx2, vanakWidth, vanakHeight);

        //var delta = 1E-7;

        //Assert.Equal(vanakWorldfile.XPixelSize, calculatedVanakWorldfile.XPixelSize, delta);
        //Assert.Equal(vanakWorldfile.YPixelSize, calculatedVanakWorldfile.YPixelSize, delta);
        //Assert.Equal(vanakWorldfile.CenterOfUpperLeftPixel.X, calculatedVanakWorldfile.CenterOfUpperLeftPixel.X, delta);
        //Assert.Equal(vanakWorldfile.CenterOfUpperLeftPixel.Y, calculatedVanakWorldfile.CenterOfUpperLeftPixel.Y, delta);
    }

    public void Test(string fileName, int width, int height)
    {
        var vanakBbx1 = WorldfileManager.ReadImageBoundingBox(fileName, width, height);

        var vanakWorldfile = Worldfile.Read(fileName);

        var vanakBbx2 = vanakWorldfile.GetBoundingBox(width, height);

        Assert.Equal(vanakBbx1, vanakBbx2);

        var calculatedVanakWorldfile = WorldfileManager.Create(vanakBbx2, width, height);

        //var delta = 1E-7;
        int delta = 7;

        Assert.Equal(vanakWorldfile.XPixelSize, calculatedVanakWorldfile.XPixelSize, delta);
        Assert.Equal(vanakWorldfile.YPixelSize, calculatedVanakWorldfile.YPixelSize, delta);
        Assert.Equal(vanakWorldfile.CenterOfUpperLeftPixel.X, calculatedVanakWorldfile.CenterOfUpperLeftPixel.X, delta);
        Assert.Equal(vanakWorldfile.CenterOfUpperLeftPixel.Y, calculatedVanakWorldfile.CenterOfUpperLeftPixel.Y, delta);
    }
}
