using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess
{
    /// <summary>
    /// Represents the Dialect used to communicate with a database.
    /// </summary>
    public class Dialect
    {

        #region Properties

        /// <summary>
        /// The list of reserved words for the dialect
        /// </summary>
        public List<string> ReservedWords { get; protected set; }

        /// <summary>
        /// The Character to add before and after any field. Add two characters to consider the first for the beginning and the second for the end. 
        /// Use it for databases where you add field names that follow this example: [myFieldName].
        /// </summary>
        public string FieldNameChar { get; protected internal set; }

        /// <summary>
        /// The Character to add before and after any text field. Use it to add " or ' before any text assignment. The default value is '.
        /// </summary>
        public string FieldValueChar { get; protected internal set; }

        /// <summary>
        /// The Character to add before and after any table name. Add two characters to consider the first for the beginning and the second for the end. 
        /// Use it for databases where you add table names that follow this example: [myTableName].
        /// </summary>
        public string TableNameChar { get; protected internal set; }

        /// <summary>
        /// The default date format to be used by the dialect
        /// </summary>
        public string DateFormat { get; protected internal set; }

        /// <summary>
        /// The default time format to be used by the dialect
        /// </summary>
        public string TimeFormat { get; protected internal set; }

        /// <summary>
        /// The default Numeric format to be used by the dialect
        /// </summary>
        public string NumericFormat { get; protected internal set; }

        /// <summary>
        /// The default Decimal format to be used by the dialect
        /// </summary>
        public string DecimalFormat { get; protected internal set; }

        /// <summary>
        /// The datetime format
        /// </summary>
        public string DateTimeFormat
        {
            get
            {
                return $"{DateFormat} {TimeFormat}";
            }
        }

        /// <summary>
        /// Any statement that might be executed right before running any query
        /// </summary>
        public List<string> BeforeQueryStatements { get; protected set; }

        /// <summary>
        /// Any statement that might be executed right after a query runs
        /// </summary>
        public List<string> AfterQueryStatements { get; protected set; }

        /// <summary>
        /// The character to be appended at the end of any statement
        /// </summary>
        public string StatementEndCharacter { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of a Dialect
        /// </summary>
        public Dialect()
        {
            // Set Default Values
            FieldNameChar = "";
            FieldValueChar = "'";
            TableNameChar = "";
            DateFormat = "yyyy-MM-dd";
            TimeFormat = "HH:mm:ss";
            NumericFormat = "";
            DecimalFormat = "";
            StatementEndCharacter = "";

            ReservedWords = new List<string>();
            BeforeQueryStatements = new List<string>();
            AfterQueryStatements = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of a Dialect
        /// </summary>
        /// <param name="fieldNameChar">The character that goes before and after any field name</param>
        /// <param name="fieldValueChar">The character that goes before and after any text field value</param>
        public Dialect(string fieldNameChar, string fieldValueChar) : this()
        {
            FieldNameChar = fieldNameChar;
            FieldValueChar = fieldValueChar;
        }

        /// <summary>
        /// Initializes a new instance of a Dialect
        /// </summary>
        /// <param name="fieldNameChar">The character that goes before and after any field name</param>
        /// <param name="fieldValueChar">The character that goes before and after any text field value</param>
        /// <param name="numericFormat">The numeric format</param>
        /// <param name="decimalFormat">The decimal format</param>
        /// <param name="dateFormat">The Date Format according to the <see cref="DateTime">DateTime formats</see></param>
        /// <param name="timeFormat">The Time Format according to the <see cref="DateTime">DateTime formats</see></param>
        public Dialect(string fieldNameChar, string fieldValueChar, string numericFormat, string decimalFormat, string dateFormat, string timeFormat) : this(fieldNameChar, fieldValueChar)
        {
            DateFormat = dateFormat;
            TimeFormat = timeFormat;
            NumericFormat = numericFormat;
            DecimalFormat = decimalFormat;
        }

        #endregion Constructors

        #region Quick Initialization Methods

        /// <summary>
        /// Adds many reserved words to the list
        /// </summary>
        /// <param name="list">List of parameters</param>
        protected void AddReservedWords(params string[] list)
        {
            ReservedWords.AddRange(list);
        }

        /// <summary>
        /// Adds many sql statements to run before any query to the list
        /// </summary>
        /// <param name="list">The list of sql statements</param>
        protected void AddBeforeStatements(params string[] list)
        {
            BeforeQueryStatements.AddRange(list);
        }

        /// <summary>
        /// Adds many sql statements to run after any query to the list
        /// </summary>
        /// <param name="list">The list of sql statements</param>
        protected void AddAfterStatements(params string[] list)
        {
            AfterQueryStatements.AddRange(list);
        }

        #endregion Quick Initialization Methods

        #region Formatting Methods / Functions

        /// <summary>
        /// Validates the Table Name. Tables may not have blank spaces inside the string, unless there is a valid table name character.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected bool ValidateTableName(string tableName)
        {
            // Remove any blanks from the string
            TableNameChar = TableNameChar.Trim();

            // Validate Table Name Char (might not have more than two characters)
            if (TableNameChar.Length > 2) return false;

            // Look for the separator inside it
            if (TableNameChar != "")
            {
                /* There is a table name char */
                /* On this case, it's acceptable to have inner blank spaces */

                if (TableNameChar.Length == 2)
                {
                    // There is a begin / end character
                    if ((tableName.Contains(TableNameChar[0].ToString())) &&
                        (tableName.Contains(TableNameChar[1].ToString())))
                    {
                        /* There is a problem , the table name should not have the separator character inside it */
                        return false;
                    }
                }
                else
                {
                    // There is only one character
                    if (tableName.Contains(TableNameChar[0].ToString()))
                    {
                        /* There is a problem , the table name should not have the separator character inside it */
                        return false;
                    }
                }
            }
            else
            {
                /* There is no table name char */
                /* On this case, it's not acceptable to have inner blank spaces */

                // Look for blank spaces
                if (tableName.Contains(" "))
                {
                    /* There are blank spaces inside it, consider invalid */
                    return false;
                }
            }

            // Everything is fine
            return true;
        }

        /// <summary>
        /// Returns the default formatting for a table name.
        /// </summary>
        /// <param name="tableOwner">The owner of the table (or library depending on the database type)</param>
        /// <param name="tableName">The table name</param>
        /// <returns>The input table name prefixed and suffixed with the <see cref="TableNameChar">table character separator</see>.</returns>
        public virtual string FormatTable(string tableOwner, string tableName)
        {
            // Check if the table name is valid
            if (!ValidateTableName(tableName)) throw new Exception("Invalid table name or table character");

            if (TableNameChar.Length == 2)
            {
                if (tableOwner?.ToString() == "")
                {
                    // Format is: [myTable]
                    return $"{TableNameChar[0]}{tableName}{TableNameChar[1]}";
                }
                else
                {
                    // Format is: [dbo].[myTable]
                    return $"{TableNameChar[0]}{tableOwner}{TableNameChar[1]}.{TableNameChar[0]}{tableName}{TableNameChar[1]}";
                }
            }
            else
            {
                if (tableOwner?.ToString() == "")
                {
                    // Format is: myTable
                    return $"{TableNameChar}{tableName}{TableNameChar}";
                }
                else
                {
                    // Format is: dbo.myTable
                    return $"{TableNameChar}{tableOwner}{TableNameChar}.{TableNameChar}{tableName}{TableNameChar}";
                }
            }
                
        }

        /// <summary>
        /// Returns the default formatting for a table name and its alias.
        /// </summary>
        /// <param name="tableOwner">The owner of the table (or library depending on the database type)</param>
        /// <param name="tableName">The table name</param>
        /// <param name="tableAlias">The table alias</param>
        /// <returns>The input table name prefixed and suffixed with the <see cref="TableNameChar">table character separator</see>, concatenated with the table alias preceeded by "T_".</returns>
        public virtual string FormatTable(string tableOwner, string tableName, string tableAlias)
        {
            // For security reasons (avoid users passing commands on the table alias), add the "T_" before the table alias.
            return $"{FormatTable(tableOwner, tableName)} T_{tableAlias}";
        }

        #endregion Formatting Methods / Functions

        #region Static Dialect Implementations

        #region Dialect - Microsoft SQL Server

        /// <summary>
        /// Static container for the MsSqlDialect, implementing the Singleton Design Pattern
        /// </summary>
        private static Dialect _msSqlDialect;
        /// <summary>
        /// Represents a Dialect to be interacting with Microsoft SQL Server Databases
        /// </summary>
        public static Dialect MsSqlDialect
        {
            get
            {
                //TODO: implement thread safe locking

                // Check for existing implementation already in place
                if (_msSqlDialect == null)
                {
                    _msSqlDialect = new Dialect()
                    {
                        FieldNameChar = "",
                        FieldValueChar = "'",
                        TableNameChar = "[]",
                        NumericFormat = "",
                        DecimalFormat = "",
                        DateFormat = "yyyy-MM-dd",
                        TimeFormat = "HH:mm:ss",                        
                    };

                    // Include Reserved Words
                    _msSqlDialect.AddReservedWords("user", 
                                                   "datetime",
                                                   "date",
                                                   "object",
                                                   "name",
                                                   "description");

                    // Include Date Formatting before any statement
                    _msSqlDialect.AddBeforeStatements("SET DATEFORMAT ymd");        // Always consider dates to be Year-Month-Date, because we do not know what is the local language and thus the format
                    _msSqlDialect.AddBeforeStatements("SET DATEFIRST 7");           // Sets Sunday to be the first day of the week
                }

                // Returns the Dialect
                return _msSqlDialect;
            }
        }

        #endregion Dialect - Microsoft SQL Server

        #region Dialect - DB2/400

        /// <summary>
        /// Static container for the Db2400, implementing the Singleton Design Pattern
        /// </summary>
        private static Dialect _db2400Dialect;
        /// <summary>
        /// Represents a Dialect to be interacting with DB2 Databases in AS400
        /// </summary>
        public static Dialect DB2400Dialect
        {
            get
            {
                //TODO: implement thread safe locking

                // Check for existing implementation already in place
                if (_db2400Dialect == null)
                {
                    _db2400Dialect = new Dialect()
                    {
                        FieldNameChar = "",
                        FieldValueChar = "'",
                        TableNameChar = "",
                        NumericFormat = "",
                        DecimalFormat = "",
                        DateFormat = "yyyy-MM-dd",
                        TimeFormat = "HHmmss",
                    };

                    // Include Reserved Words
                    _db2400Dialect.AddReservedWords("user",
                                                    "datetime",
                                                    "date",
                                                    "object",
                                                    "name",
                                                    "description");

                }

                // Returns the Dialect
                return _db2400Dialect;
            }
        }

        #endregion Dialect - DB2/400

        #endregion Static Dialect Implementations
    }
}
