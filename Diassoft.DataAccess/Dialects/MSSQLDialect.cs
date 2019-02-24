﻿using Diassoft.DataAccess.DatabaseObjects;
using Diassoft.DataAccess.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Diassoft.DataAccess.DatabaseObjects.Fields;
using Diassoft.DataAccess.DatabaseObjects.Expressions;

namespace Diassoft.DataAccess.Dialects
{
    /// <summary>
    /// Represents the Microsoft SQL Server T-SQL Dialect
    /// </summary>
    public class MSSQLDialect: Dialect
    {
        /// <summary>
        /// Initializes a new instance of the Microsoft SQL T-SQL Dialect
        /// </summary>
        public MSSQLDialect(): base()
        {
            FieldNameChar = "[]";
            FieldValueChar = "'";
            TableNameChar = "[]";
            NumericFormat = "";
            DecimalFormat = "";
            DateFormat = "yyyy-MM-dd";
            TimeFormat = "HH:mm:ss";

            // Include Reserved Words
            AddReservedWords("user",
                             "datetime",
                             "date",
                             "object",
                             "name",
                             "description");

            // Include Date Formatting before any statement
            AddBeforeStatements("SET DATEFORMAT ymd");        // Always consider dates to be Year-Month-Date, because we do not know what is the local language and thus the format
            AddBeforeStatements("SET DATEFIRST 7");           // Sets Sunday to be the first day of the week
        }

        /// <summary>
        /// Selects data from the database using the Microsoft SQL Server T-SQL Dialect
        /// </summary>
        /// <param name="select">An <see cref="SelectDbOperation"/> representing the Select to Perform</param>
        /// <returns>A <see cref="string"/> containing the select statement to be executed</returns>
        public override string Select(SelectDbOperation select)
        {
            return SelectInto(select, null);
        }

        /// <summary>
        /// Selects data from the database using the Microsoft SQL Server T-SQL Dialect. Appends the result into a table
        /// </summary>
        /// <param name="select">An <see cref="SelectDbOperation"/> representing the Select to Perform</param>
        /// <param name="intoTable">The <see cref="Table"/> where the data should be inserted</param>
        /// <returns>A <see cref="string"/> containing the select statement to be executed</returns>
        public override string SelectInto(SelectDbOperation select, Table intoTable)
        {
            // Container for Query Parameters
            StringBuilder sbSelect = new StringBuilder();

            // Ensure there are tables on the operation
            if (select.Tables == null) throw new Exception("There are no tables on the Select Statement");
            if (select.Tables.Length == 0) throw new Exception("There are no tables on the Select Statement");

            // Make sure Distinct and Group by are not activated at the same time
            if ((select.Distinct) && (select.GroupBy))
                throw new Exception($"Unable to make a Select Statement with both DISTINCT and GROUP BY at the same time");

            // Check Group By Setup
            if (select.GroupBy)
            {
                if ((select.SelectFields == null) || (select.SelectFields?.Length == 0))
                    throw new Exception($"When using Group By you need to inform the columns you want to group on the {nameof(select.SelectFields)} collection.");
            }

            // ====================================================================================
            // SELECT FIELDS AREA
            // ====================================================================================

            // Display Fields
            sbSelect.Append("SELECT ");

            if (select.Distinct) sbSelect.Append("DISTINCT ");

            // Check all Non-Aggregate Fields
            if (select.SelectFields?.Length > 0)
            {
                // For formatting purposes, add a line right after the select
                sbSelect.AppendLine();

                // Select Specific Fields
                sbSelect.AppendLine(String.Join(",\r\n", from field in @select.SelectFields
                                                         select String.Concat(String.Empty.PadLeft(7, ' '),
                                                                              this.FormatExpressionField(field))));
            }
            else
            {
                // Select All Fields
                sbSelect.Append('*');
                sbSelect.AppendLine();
            }

            // ====================================================================================
            // INTO AREA
            // ====================================================================================

            // If a table was passed, use it for a Select Into
            if (intoTable != null)
            {
                sbSelect.AppendLine("  INTO");
                sbSelect.AppendLine(base.FormatTable(intoTable).PadLeft(7, ' '));
            }

            // ====================================================================================
            // FROM AREA
            // ====================================================================================

            // From Statement
            sbSelect.Append("  FROM ");

            if (select.Tables.Length == 1)
            {
                // Append Single Table
                sbSelect.AppendLine(FormatTable(select.Tables[0]));
            }
            else
            {
                // Append Multiple Tables Separated by a Comma
                sbSelect.AppendLine();
                sbSelect.AppendLine(String.Join(",\r\n", from tbl in @select.Tables
                                                         select String.Concat(String.Empty.PadLeft(7, ' '), FormatTable(tbl))));
            }

            // ====================================================================================
            // JOINS AREA
            // ====================================================================================

            //TODO: Implement 

            // ====================================================================================
            // WHERE AREA
            // ====================================================================================

            if (select.Where?.Length > 0)
            {
                sbSelect.AppendLine(" WHERE");
                sbSelect.Append(FormatExpressions(select.Where, 1));
            }

            // ====================================================================================
            // GROUP BY AREA
            // ====================================================================================

            // Group By Fields
            if (select.GroupBy)
            {
                sbSelect.AppendLine("GROUP BY");

                if (select.SelectFields?.Count(field => field.GetType() != typeof(AggregateField)) > 0)
                {
                    // Add all display fields but make sure to not have the alternate name on it
                    sbSelect.AppendLine(String.Join(",\r\n", from field in @select.SelectFields
                                                             where field.GetType() != typeof(AggregateField)
                                                             select String.Concat(String.Empty.PadLeft(9, ' '), FormatField(new DisplayField(((DisplayField)field).Name, ((DisplayField)field).TableAlias)))));
                }
            }

            // ====================================================================================
            // HAVING AREA
            // ====================================================================================

            //TODO: Implement

            // Returns the Result of the Select Statement
            return sbSelect.ToString();

        }




    }
}
