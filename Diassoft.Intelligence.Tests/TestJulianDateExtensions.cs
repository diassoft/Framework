using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diassoft.Intelligence.Extensions;


namespace Diassoft.Intelligence.Tests
{
    [TestClass]
    public class TestJulianDateExtensions
    {
        [TestMethod]
        public void TestToJulian_001()
        {            
            Assert.AreEqual<int>(110001, 
                                 new DateTime(2010, 1, 1).ToJulian());

            Assert.AreEqual<int>(117021,
                                 new DateTime(2017, 1, 21).ToJulian());

        }

        [TestMethod]
        public void TestFromJulian_002()
        {
            Assert.AreEqual<DateTime>(new DateTime(2010, 1, 1), new DateTime().CreateFromJulian(110001));
            Assert.AreEqual<DateTime>(new DateTime(2017, 1, 21), new DateTime().CreateFromJulian(117021));

        }
    }
}
