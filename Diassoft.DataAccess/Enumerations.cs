using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess
{
    /// <summary>
    /// Database Dialects
    /// This defines how the data is parsed for the sql statement to be created
    /// </summary>
    public enum DialectsA : int
    {
        /// <summary>
        /// Microsoft Access Database
        /// </summary>
        Access = 0,
        /// <summary>
        /// Microsoft SQL Server Databases
        /// </summary>
        SQL = 1,
        /// <summary>
        /// Oracle Databases
        /// </summary>
        Oracle = 2,
        /// <summary>
        /// MySql Databases
        /// </summary>
        MySql = 3,
        /// <summary>
        /// DB2/400 Databases
        /// </summary>
        DB2 = 4,
        /// <summary>
        /// A Generic OleDb
        /// </summary>
        GenericOleDb = 20,
        /// <summary>
        /// A Generic ODBC
        /// </summary>
        GenericODBC = 21,
        /// <summary>
        /// Any other database
        /// </summary>
        Other = 99
    }
}
