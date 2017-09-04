using Diassoft.DataAccess.DatabaseObjects;
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

        #region Table Formatting

        /// <summary>
        /// Validates the Table Object. 
        /// </summary>
        /// <param name="table">A <see cref="Table"/> containing the database table information</param>
        /// <remarks>Tables may not have blank spaces inside the string, unless there is a valid table name character.</remarks>
        /// <returns></returns>
        protected bool ValidateTable(Table table)
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
                    if ((table.Name.Contains(TableNameChar[0].ToString())) &&
                        (table.Name.Contains(TableNameChar[1].ToString())))
                    {
                        /* There is a problem , the table name should not have the separator character inside it */
                        return false;
                    }
                }
                else
                {
                    // There is only one character
                    if (table.Name.Contains(TableNameChar[0].ToString()))
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
                if (table.Name.Contains(" "))
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
        /// <param name="table">An instance of a <see cref="Table"/> object to represent the database table</param>
        /// <returns>The input table name prefixed and suffixed with the <see cref="TableNameChar">table character separator</see>.</returns>
        public virtual string FormatTable(Table table)
        {
            // Variable to hold the Table Name
            string FormattedTableName = "";
            string CharacterBegin = "";
            string CharacterEnd = "";

            // Check if the table name is valid
            if (!ValidateTable(table)) throw new Exception("Invalid table name or table character");

            if (TableNameChar.Length == 2)
            {
                // Set Character Begin / End
                CharacterBegin = TableNameChar[0].ToString();
                CharacterEnd = TableNameChar[1].ToString();
            }
            else if (TableNameChar.Length == 1)
            {
                // Set Character Begin / End (the same)
                CharacterBegin = TableNameChar[0].ToString();
                CharacterEnd = TableNameChar[0].ToString();
            }

            // Verify Owner
            if (table?.Owner?.ToString() != "")
            {
                // There is an owner, add it to the string
                FormattedTableName += $"{CharacterBegin}{table.Owner}{CharacterEnd}.";
            }

            // Add the table name
            FormattedTableName += $"{CharacterBegin}{table.Name}{CharacterEnd}";

            // Verify Alias
            if (table?.Alias?.ToString() != "")
            {
                // For security reasons (avoid users passing additional commands on the table alias), add the "T_" before the table alias.
                FormattedTableName += $" T_{table.Alias}";
            }

            return FormattedTableName;
        }

        /// <summary>
        /// Formats the table
        /// </summary>
        /// <param name="table">An instance of a <see cref="Table"/> object to represent the database table</param>
        /// <param name="formattedTable">Output parameter where the formatted table will be stored</param>
        /// <returns>A <see cref="bool">bool</see> value to define whether the table formatting was successfull or not.</returns>
        public bool TryFormatTable(Table table, out string formattedTable)
        {
            try
            {
                // Format the table
                formattedTable = FormatTable(table);
                return true;
            }
            catch
            {
                // Formatting Failed
                formattedTable = "";
                return false;
            }
        }

        /// <summary>
        /// Returns the default formatting for a table name.
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="tableOwner">The owner of the table (or library depending on the database type)</param>
        /// <returns>The input table name prefixed and suffixed with the <see cref="TableNameChar">table character separator</see>.</returns>
        public virtual string FormatTable(string tableName, string tableOwner)
        {
            // For security reasons (avoid users passing commands on the table alias), add the "T_" before the table alias.
            return $"{FormatTable(new Table(tableName, tableOwner))}";
        }

        /// <summary>
        /// Formats the table
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="tableOwner">The owner of the table (or library depending on the database type)</param>
        /// <param name="formattedTable">Output parameter where the formatted table will be stored</param>
        /// <returns>A <see cref="bool">bool</see> value to define whether the table formatting was successfull or not.</returns>
        public bool TryFormatTable(string tableName, string tableOwner, out string formattedTable)
        {
            try
            {
                // Format the table
                formattedTable = FormatTable(tableName, tableOwner);
                return true;
            }
            catch
            {
                // Formatting Failed
                formattedTable = "";
                return false;
            }
        }


        /// <summary>
        /// Returns the default formatting for a table name and its alias.
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="tableOwner">The owner of the table (or library depending on the database type)</param>
        /// <param name="tableAlias">The table alias</param>
        /// <returns>The input table name prefixed and suffixed with the <see cref="TableNameChar">table character separator</see>, concatenated with the table alias preceeded by "T_".</returns>
        public virtual string FormatTable(string tableName, string tableOwner, string tableAlias)
        {
            return $"{FormatTable(new Table(tableName, tableOwner, tableAlias))}";
        }

        /// <summary>
        /// Formats the table
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="tableOwner">The owner of the table (or library depending on the database type)</param>
        /// <param name="tableAlias">The table alias</param>
        /// <param name="formattedTable">Output parameter where the formatted table will be stored</param>
        /// <returns>A <see cref="bool">bool</see> value to define whether the table formatting was successfull or not.</returns>
        public bool TryFormatTable(string tableName, string tableOwner, string tableAlias, out string formattedTable)
        {
            try
            {
                // Format the table
                formattedTable = FormatTable(tableName, tableOwner, tableAlias);
                return true;
            }
            catch 
            {
                // Formatting Failed
                formattedTable = "";
                return false;
            }
        }

        #endregion Table Formatting

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
