using IRI.Ket.WorldfileFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.Common
{
    [TestClass]
    public class WorldfileFormatTest
    {
        string vanakUtm = $"Assets\\WorldfileSamples\\vanakUTM.tfw";
        int vanakHeight = 2095;
        int vanakWidth = 2881;


        string Ni = $"Assets\\WorldfileSamples\\NI38.8.jpgw";
        int niHeight = 3017;
        int niWidth = 3784;

        [TestMethod]
        public void TestWorldfile()
        {
            Test(vanakUtm, vanakWidth, vanakHeight);

            Test(Ni, niWidth, niHeight);
            //var vanakBbx1 = WorldfileManager.ReadImageBoundingBox(vanakUtm, vanakHeight, vanakWidth);

            //var vanakWorldfile = Worldfile.Read(vanakUtm);

            //var vanakBbx2 = vanakWorldfile.GetBoundingBox(vanakWidth, vanakHeight);

            //Assert.AreEqual(vanakBbx1, vanakBbx2);

            //var calculatedVanakWorldfile = WorldfileManager.Create(vanakBbx2, vanakWidth, vanakHeight);

            //var delta = 1E-7;

            //Assert.AreEqual(vanakWorldfile.XPixelSize, calculatedVanakWorldfile.XPixelSize, delta);
            //Assert.AreEqual(vanakWorldfile.YPixelSize, calculatedVanakWorldfile.YPixelSize, delta);
            //Assert.AreEqual(vanakWorldfile.CenterOfUpperLeftPixel.X, calculatedVanakWorldfile.CenterOfUpperLeftPixel.X, delta);
            //Assert.AreEqual(vanakWorldfile.CenterOfUpperLeftPixel.Y, calculatedVanakWorldfile.CenterOfUpperLeftPixel.Y, delta);
        }

        public void Test(string fileName, int width, int height)
        {
            var vanakBbx1 = WorldfileManager.ReadImageBoundingBox(fileName, width, height);

            var vanakWorldfile = Worldfile.Read(fileName);

            var vanakBbx2 = vanakWorldfile.GetBoundingBox(width, height);

            Assert.AreEqual(vanakBbx1, vanakBbx2);

            var calculatedVanakWorldfile = WorldfileManager.Create(vanakBbx2, width, height);

            var delta = 1E-7;

            Assert.AreEqual(vanakWorldfile.XPixelSize, calculatedVanakWorldfile.XPixelSize, delta);
            Assert.AreEqual(vanakWorldfile.YPixelSize, calculatedVanakWorldfile.YPixelSize, delta);
            Assert.AreEqual(vanakWorldfile.CenterOfUpperLeftPixel.X, calculatedVanakWorldfile.CenterOfUpperLeftPixel.X, delta);
            Assert.AreEqual(vanakWorldfile.CenterOfUpperLeftPixel.Y, calculatedVanakWorldfile.CenterOfUpperLeftPixel.Y, delta);
        }
    }
}
