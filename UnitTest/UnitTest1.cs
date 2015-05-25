using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataBaseGenerator;

namespace UnitTest
{
    [TestClass]
    public class DbGeneratorUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            DbGenerator gen = new DbGenerator();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.IsTrue(true);
        }
    }
}
