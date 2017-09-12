using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diassoft.Intelligence.Translators;

namespace Diassoft.Intelligence.Tests
{
    [TestClass]
    public class TestStringToDateTranslator
    {
        [TestMethod]
        public void TestTranslate_001()
        {
            StringToDateTranslator translator = new StringToDateTranslator();

            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("July27th1984"));


        }
    }
}
