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
            string expectedStatement = "SELECT *\r\n  FROM [dbo].[testtable] T_0\r\n";

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
    }
}
