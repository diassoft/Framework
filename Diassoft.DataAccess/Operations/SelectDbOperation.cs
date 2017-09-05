using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.DataAccess.DatabaseObjects;
using Diassoft.DataAccess.DatabaseObjects.Fields;
using Diassoft.DataAccess.DatabaseObjects.Expressions;
using System.Data;
using System.Linq;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// A class that represents a Select database operation
    /// </summary>
    public class SelectDbOperation: InquiryDbOperation
    {

        #region Constructors

        /* Since this is using a inheritance structure, it's necessary to redefine the methods on this base class.
         * It's going to call the Base Methods always, there is no need to implement any logic here */

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        protected SelectDbOperation() : base() { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected SelectDbOperation(Dialect dialect): base(dialect) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection): base(dialect, connection) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): base(dialect, connection, transaction) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectDbOperation(Dialect dialect, Table table) : base(dialect, table) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, Table table) : base(dialect, connection, table) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table table) : base(dialect, connection, transaction, table) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected SelectDbOperation(Dialect dialect, params Table[] tables) : base(dialect, tables) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, params Table[] tables) : base(dialect, connection, tables) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table[] tables) : base(dialect, connection, transaction, tables) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Defines whether the select will be of the distinct fields.
        /// By default the value is false.
        /// </summary>
        public bool Distinct { get; set; } = false;

        /// <summary>
        /// Defines the Top N Rows to be displayed.
        /// By default the value is zero.
        /// </summary>
        public int TopNRows { get; set; } = 0;

        /// <summary>
        /// Defines whether there will be a Group By at the end of the query
        /// </summary>
        public bool GroupBy { get; set; } = false;

        /// <summary>
        /// A list with the fields to be selected, including aggregates
        /// </summary>
        public List<DisplayField> SelectFields { get; set; } = new List<DisplayField>();

        /// <summary>
        /// List of Filter Expressions
        /// </summary>
        public List<Expression> Where { get; set; } = new List<Expression>();

        #endregion Properties

        #region Methods / Functions

        /// <summary>
        /// Validates the Conditions for the Statement Execution
        /// </summary>
        public new void PreStatementValidation()
        {
            // Calls the Base PreStatementValidation (This already takes care of validating the number of tables)
            base.PreStatementValidation();

            // Make sure Distinct and Group by are not activated at the same time
            if (Distinct && GroupBy) throw new Exception($"Unable to make a Select Statement with both DISTINCT and GROUP BY activated.");
            
            // Check Group By Setup
            if (GroupBy)
            {
                if ((SelectFields == null) ||
                    (SelectFields?.Count == 0)) throw new Exception($"When using Group By you need to inform the columns you want to group on the {nameof(SelectFields)} collection.");
            }
        }

        /// <summary>
        /// Returns the Statement for the <see cref="SelectDbOperation"/>.
        /// </summary>
        /// <returns>A string containing the select statement</returns>
        public override string GetStatement()
        {
            // Make sure it's ok to run the statement 
            this.PreStatementValidation();

            // For performance issues, use the String Builder Class 
            StringBuilder sbStatement = new StringBuilder();

            #region Select

            // Display Fields
            StringBuilder sbSelect = new StringBuilder("SELECT ");

            if (Distinct) sbSelect.Append("DISTINCT ");

            // Check all Non-Aggregate Fields
            if (SelectFields?.Count(field => field.Type != FieldTypes.Aggregate) == 0)
            {
                // Select All Fields
                sbSelect.Append('*');
            }
            else
            {
                // Select Specific Fields (Ignore Aggregates)
                sbSelect.Append(String.Join(",\n", from field in SelectFields
                                                   where field.Type != FieldTypes.Aggregate
                                                   select String.Concat(String.Empty.PadLeft(7, ' '), Dialect.FormatField(field))));
            }

            #endregion Select

            #region Select Aggregates

            // Check all Aggregate Fields
            if (SelectFields?.Count(Field => Field.Type == FieldTypes.Aggregate) > 0)
            {
                sbSelect.Append(String.Join(",\n", from field in SelectFields
                                                   where field.Type == FieldTypes.Aggregate && field is AggregateField
                                                   select String.Concat(String.Empty.PadLeft(7, ' '), Dialect.FormatAggregateField((AggregateField)field))));
            }

            #endregion Select Aggregates

            #region From

            // From Statement
            sbSelect.Append("  FROM ");

            if (base.Tables == null) throw new NullReferenceException($"Tables are not defined for the {nameof(SelectDbOperation)}.");
            if (base.Tables.Count == 0) throw new Exception($"There are no tables assigned for the {nameof(SelectDbOperation)}.");

            if (base.Tables?.Count == 1)
            {
                // Append Single Table
                sbSelect.AppendLine(base.Dialect.FormatTable(base.Tables[0]));
            }
            else
            {
                // Append Multiple Tables Separated by a Comma
                sbSelect.AppendLine();
                sbSelect.Append(String.Join(",\n", from tbl in base.Tables
                                                   select String.Concat(String.Empty.PadLeft(7, ' '), Dialect.FormatTable(tbl))));
            }

            #endregion From

            #region Joins



            #endregion Joins

            #region Where

            // Add Filters, if needed
            if (Where?.Count > 0)
            {
                sbSelect.AppendLine(" WHERE ");

            }

            #endregion Where

            #region Group By

            // Group By Fields
            if (GroupBy)
            {
                sbSelect.AppendLine("GROUP BY");

                if (SelectFields?.Count(field => field.Type != FieldTypes.Aggregate) > 0)
                {
                    sbSelect.AppendLine(String.Join(",\n", from item in SelectFields
                                                           where item.Type != FieldTypes.Aggregate
                                                           select String.Concat(String.Empty.PadLeft(9,' '), Dialect.FormatField(item))));
                }
            }

            #endregion Group By

            // Return the Final Statement
            return sbSelect.ToString();
        }

        #endregion Methods / Functions

    }
}
