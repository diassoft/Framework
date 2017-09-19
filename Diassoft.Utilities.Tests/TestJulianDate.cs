using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Diassoft.Utilities.Tests
{
    [TestClass]
    public class TestJulianDate
    {

        public class JulianDateTestData
        {
            public DateTime gregorian { get; set; }
            public int julian { get; set; }
            public bool Exception { get; set; }

            public JulianDateTestData()
            {

            }

            public JulianDateTestData(DateTime pGregorian, int pJulian)
            {
                gregorian = pGregorian;
                julian = pJulian;
                Exception = false;
            }

            public JulianDateTestData(DateTime pGregorian, int pJulian, bool pException) : this(pGregorian, pJulian)
            {
                Exception = pException;
            }
        }


        /// <summary>
        /// Test Data Array
        /// </summary>
        private static List<JulianDateTestData> _testData = new List<JulianDateTestData>()
        {
            new JulianDateTestData(DateTime.MinValue, 0),
            new JulianDateTestData(new DateTime(2017, 1, 1), 117001),
            new JulianDateTestData(new DateTime(2017, 1, 2), 117002),
            new JulianDateTestData(new DateTime(2017, 1, 3), 117003),
            new JulianDateTestData(new DateTime(1998, 1, 5), 98005),
            new JulianDateTestData(new DateTime(1991, 1, 3), 91003),
            new JulianDateTestData(new DateTime(1930, 1, 10), 30010),
            new JulianDateTestData(new DateTime(1996, 12, 31), 96366),
        };

        private static List<JulianDateTestData> _testDataJulianToGregorian = new List<JulianDateTestData>()
        {
            new JulianDateTestData(DateTime.MinValue, 117366, true)
        };

        private static List<JulianDateTestData> _testDataGregorianToJulian = new List<JulianDateTestData>()
        {
            new JulianDateTestData(new DateTime(1800, 1, 1), 0, true)
        };

        [TestMethod]
        public void TestConvertJulianToGregorian()
        {
            List<JulianDateTestData> testDataList = new List<JulianDateTestData>();
            testDataList.AddRange(_testData);
            testDataList.AddRange(_testDataJulianToGregorian);

            foreach (JulianDateTestData testData in testDataList)
            {
                // Create the gregorian date
                try
                {
                    DateTime gregorian = JulianDate.ConvertToGregorian(testData.julian);
                    Assert.AreEqual<DateTime>(testData.gregorian, gregorian);

                    Assert.AreEqual<bool>(testData.Exception, false);
                }
                catch
                {
                    Assert.AreEqual<bool>(testData.Exception, true);
                }
            }
        }

        [TestMethod]
        public void TestConvertGregorianToJulian()
        {
            List<JulianDateTestData> testDataList = new List<JulianDateTestData>();
            testDataList.AddRange(_testData);
            testDataList.AddRange(_testDataGregorianToJulian);

            foreach (JulianDateTestData testData in TestJulianDate._testData)
            {
                // Create the julian date
                try
                {
                    int julian = JulianDate.ConvertToJulian(testData.gregorian);
                    Assert.AreEqual<int>(testData.julian, julian);

                    Assert.AreEqual<bool>(testData.Exception, false);
                }
                catch (Exception)
                {
                    Assert.AreEqual<bool>(testData.Exception, true);
                }
            }
        }

        [TestMethod]
        public void TestJulianDateClass()
        {
            // Test Property Assignment
            foreach (JulianDateTestData testData in _testData)
            {
                Assert.AreEqual<DateTime>(testData.gregorian, JulianDate.ConvertToGregorian(testData.julian));
                Assert.AreEqual<int>(testData.julian, JulianDate.ConvertToJulian(testData.gregorian));
            }

            //// Test Erroneous Julian Date Assignments
            //foreach (JulianDateTestData testData in _testDataJulianToGregorian)
            //{
            //    jd = new JulianDate();
            //    jd.Julian = testData.julian;
            //    Assert.AreEqual<bool>(false, jd.IsValidDate);
            //}

            //// Test Erroneous Gregorian Date Assignments
            //foreach (JulianDateTestData testData in _testDataGregorianToJulian)
            //{
            //    jd = new JulianDate();
            //    jd.Gregorian = testData.gregorian;
            //    Assert.AreEqual<bool>(false, jd.IsValidDate);
            //}

        }


    }
}
