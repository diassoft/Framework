using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Diassoft.DataAccess.DatabaseObjects;
using Diassoft.DataAccess.DatabaseObjects.Fields;
using Diassoft.DataAccess.Operations;

namespace Diassoft.DataAccess.Tests
{
    [TestClass]
    public class DatabaseOperationsTest
    {
        // Dialect to be used among all testing
        Dialect myDialect = Dialect.MsSqlDialect;

        [TestMethod]
        public void TestSelectDbOperation_001_SimpleSelect()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(myDialect, new Table("testtable", "dbo", "0"));

            string statement = select.GetStatement();
            string expectedStatement = "SELECT *\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_002_SimpleSelectWithFields()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(myDialect, new Table("testtable", "dbo", "0"))
            {
                SelectFields = new List<DisplayField>()
                {
                    new DisplayField("field1", "0", "field1_alternate"),
                    new DisplayField("field2", "0", "field2_alternate"),
                }
            };

            string statement = select.GetStatement();
            string expectedStatement = "SELECT \r\n" + 
                                       "       T_0.[field1] field1_alternate,\r\n" +
                                       "       T_0.[field2] field2_alternate\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_003_SelectCountZero()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(myDialect, new Table("testtable", "dbo", "0"))
            {
                SelectFields = new List<DisplayField>()
                {
                    new AggregateField(AggregateFunctions.Count, "0")
                }
            };

            string statement = select.GetStatement();
            string expectedStatement = "SELECT \r\n" +
                                       "       COUNT(0)\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_004_SelectCountStart()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(myDialect, new Table("testtable", "dbo", "0"))
            {
                SelectFields = new List<DisplayField>()
                {
                    new AggregateField(AggregateFunctions.Count, "*")
                }
            };

            string statement = select.GetStatement();
            string expectedStatement = "SELECT \r\n" +
                                       "       COUNT(*)\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_005_SelectWithMultipleTables()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(myDialect, new Table("testtable", "dbo", "0"), new Table("testtable2", "dbo", "1"));

            string statement = select.GetStatement();
            string expectedStatement = "SELECT *\r\n" +
                                       "  FROM \r\n" +
                                       "       [dbo].[testtable] T_0,\r\n" +
                                       "       [dbo].[testtable2] T_1\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_006_SelectWithFieldAndGroupBy()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(myDialect, new Table("testtable", "dbo", "0"))
            {
                SelectFields = new List<DisplayField>()
                {
                    new DisplayField("field1", "0", "field1_alternate"),
                    new DisplayField("field2", "0"),
                    new AggregateField(AggregateFunctions.Max, "field3", "0"),
                    new AggregateField(AggregateFunctions.Min, "field4"),
                    new AggregateField(AggregateFunctions.CountDistinct, "field5", "0"),
                    new AggregateField(AggregateFunctions.Average, "field6", "0"),
                    new AggregateField(AggregateFunctions.Sum, "field7"),
                },
                GroupBy = true
            };

            string statement = select.GetStatement();
            string expectedStatement = "SELECT \r\n" +
                                       "       T_0.[field1] field1_alternate,\r\n" +
                                       "       T_0.[field2],\r\n" +
                                       "       MAX(T_0.[field3]),\r\n" +
                                       "       MIN([field4]),\r\n" +
                                       "       COUNT(DISTINCT T_0.[field5]),\r\n" +
                                       "       AVG(T_0.[field6]),\r\n" +
                                       "       SUM([field7])\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n" +
                                       "GROUP BY\r\n" +
                                       "         T_0.[field1],\r\n" +
                                       "         T_0.[field2]\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

    }
}
