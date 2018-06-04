using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.Common.Mapping
{
    [TestClass]
    public class MapIndexesTest
    {
        [TestMethod]
        public void TestMapIndexes()
        {
            var sheet = IRI.Msh.Common.Mapping.MapIndexes.Get100kIndexSheet("5261");

            Assert.AreEqual(46, sheet.Extent.XMin);
            Assert.AreEqual(31, sheet.Extent.YMin);            
        }
    }
}
