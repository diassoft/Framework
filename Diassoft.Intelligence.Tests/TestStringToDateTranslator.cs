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
            StringToDateTranslator translator = new StringToDateTranslator("en-US");

            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("July271984"));
            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("July 27 1984"));
            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("07/27/1984"));
            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("07/27/84"));
            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("7/27/84"));

            translator = new StringToDateTranslator("pt-BR");

            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("27Julho1984"));
            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("27/07/1984"));
            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("27/7/1984"));
            //Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("27 7 1984"));

            translator = new StringToDateTranslator("tk-TM");

            Assert.AreEqual<DateTime>(new DateTime(1984, 7, 27), translator.Translate("27/07/1984"));

            translator = new StringToDateTranslator();

            Assert.AreEqual<DateTime>(DateTime.Now.Date, translator.Translate("T"));
            Assert.AreEqual<DateTime>(DateTime.Now.Date, translator.Translate("t"));

        }
    }
}
