using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.Common
{
    [TestClass]
    public class WebMercatorUtilityTest
    {
        [TestMethod]
        public void TestLatLongToImageNumber()
        {
            var imageNumber = IRI.Sta.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(66.5, 89.9999, 2);
            Assert.AreEqual(2, imageNumber.X);
            Assert.AreEqual(1, imageNumber.Y);
             

            var imageNumber2 = IRI.Sta.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(0.01, -45.1, 3);
            Assert.AreEqual(2, imageNumber2.X);
            Assert.AreEqual(3, imageNumber2.Y);


            var imageNumber3 = IRI.Sta.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(-31.953, 22.501, 7);
            Assert.AreEqual(72, imageNumber3.X);
            Assert.AreEqual(76, imageNumber3.Y);


            var imageNumber4 = IRI.Sta.Common.Mapping.WebMercatorUtility.LatLonToImageNumber(27.197, 60.678, 15);
            Assert.AreEqual(21907, imageNumber4.X);
            Assert.AreEqual(13809, imageNumber4.Y);
        }
    }
}
