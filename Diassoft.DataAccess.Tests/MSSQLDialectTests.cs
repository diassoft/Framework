using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Diassoft.DataAccess.DatabaseObjects;
using Diassoft.DataAccess.DatabaseObjects.Fields;
using Diassoft.DataAccess.Operations;
using Diassoft.DataAccess.Dialects;
using Diassoft.DataAccess.DatabaseObjects.Expressions;

namespace Diassoft.DataAccess.Tests
{
    [TestClass]
    public class MSSQLDialectTests
    {
        // Dialect to be used among all testing
        MSSQLDialect myDialect = new MSSQLDialect();

        [TestMethod]
        public void TestSelectDbOperation_001_SimpleSelect()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"));

            string statement = myDialect.Select(select);
            string expectedStatement = "SELECT *\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_002_SimpleSelectWithFields()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"))
            {
                SelectFields = new object[]
                {
                    new DisplayField("field1", "0", "field1_alternate"),
                    new DisplayField("field2", "0", "field2_alternate"),
                }
            };


            string statement = myDialect.Select(select);
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
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"))
            {
                SelectFields = new object[]
                {
                    new AggregateField(Aggregates.Count, "0")
                }
            };

            string statement = myDialect.Select(select);
            string expectedStatement = "SELECT \r\n" +
                                       "       COUNT(0)\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_004_SelectCountStar()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"))
            {
                SelectFields = new object[]
                {
                    new AggregateField(Aggregates.Count, "*")
                }
            };

            string statement = myDialect.Select(select);
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
            SelectDbOperation select = new SelectDbOperation()
            {
                Tables = new object[]
                {
                    new Table("testtable", "dbo", "0"),
                    new Table("testtable2", "dbo", "1")
                }
            };

            string statement = myDialect.Select(select);
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
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"))
            {
                SelectFields = new object[]
                {
                    new DisplayField("field1", "0", "field1_alternate"),
                    new DisplayField("field2", "0"),
                    new AggregateField(Aggregates.Max, "field3", "0"),
                    new AggregateField(Aggregates.Min, "field4"),
                    new AggregateField(Aggregates.CountDistinct, "field5", "0"),
                    new AggregateField(Aggregates.Average, "field6", "0"),
                    new AggregateField(Aggregates.Sum, "field7"),
                },
                GroupBy = true
            };

            string statement = myDialect.Select(select);
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

        [TestMethod]
        public void TestSelectDbOperation_007_SelectWithWhereSingleExpression()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"))
            {
                Where = new object[]
                {
                    new Expression(new Field("field1"), FieldOperators.Equal, "Test", FieldAndOr.And),
                    new Expression(new Field("field2"), FieldOperators.GreaterThan, 15, FieldAndOr.And),
                    new Expression(new Field("field3"), FieldOperators.LessThan, 15, FieldAndOr.None),
                }
            };

            string statement = myDialect.Select(select);
            string expectedStatement = "SELECT *\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n" +
                                       " WHERE\r\n" +
                                       "       [field1]='Test' AND\r\n" +
                                       "       [field2]>15 AND\r\n" +
                                       "       [field3]<15\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }

        [TestMethod]
        public void TestSelectDbOperation_008_SelectWithWhereMultipleExpression()
        {
            // Initializes the Select Statement
            SelectDbOperation select = new SelectDbOperation(new Table("testtable", "dbo", "0"))
            {
                Where = new object[]
                {
                    new Expression(new Field("field1"), FieldOperators.Equal, "Test", FieldAndOr.And),
                    new Expression(new Field("field2"), FieldOperators.GreaterThan, 15, FieldAndOr.And),
                    new Expression(new Field("field3"), FieldOperators.LessThan, 15, FieldAndOr.And),
                    new Expression(new Field("field4"), FieldOperators.Equal, new DateTime(2020, 1, 1), FieldAndOr.And),
                    new Expression[]
                    {
                        new Expression(new Field("field5"), FieldOperators.Equal, "Test", FieldAndOr.Or),
                        new Expression(new Field("field5"), FieldOperators.Equal, "Test2", FieldAndOr.And),
                    },
                    new Expression(new Field("field6"), FieldOperators.Equal, new DateTime(2020, 1, 1), FieldAndOr.None),
                }
            };

            string statement = myDialect.Select(select);
            string expectedStatement = "SELECT *\r\n" +
                                       "  FROM [dbo].[testtable] T_0\r\n" +
                                       " WHERE\r\n" +
                                       "       [field1]='Test' AND\r\n" +
                                       "       [field2]>15 AND\r\n" +
                                       "       [field3]<15 AND\r\n" +
                                       "       [field4]='2020-01-01 00:00:00' AND\r\n" +
                                       "       (\r\n" +
                                       "              [field5]='Test' OR\r\n" +
                                       "              [field5]='Test2'\r\n" +
                                       "       ) AND\r\n" +
                                       "       [field6]='2020-01-01 00:00:00'\r\n";

            // Assertion
            Assert.AreEqual<string>(expectedStatement, statement);
        }
    }
}
