using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Diassoft.DataAccess
{
    /// <summary>
    /// Represents the Base of Any Database Operation
    /// </summary>
    public abstract class OperationBase
    {
        /// <summary>
        /// Represents the alias to refer to the Table.
        /// An alias is usually necessary when you have the same table twice on an statement.
        /// </summary>
        public string TableAlias { get; set; }

        private string _tableName;

        /// <summary>
        /// The Table Name
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// The Formatted Table Name, including the alias
        /// </summary>
        public string FullTableName
        {
            get
            {
                if (TableAlias?.ToString() != "")
                    return $"{TableName} {TableAlias}";
                else
                    return $"{TableName}";
            }
        }

        /// <summary>
        /// Creates the SQL Statement
        /// </summary>
        /// <returns></returns>
        public abstract string GetStatement();

        /// <summary>
        /// Reference to the Database Connection in use
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// Reference to the Database Transaction in use
        /// </summary>
        public IDbTransaction Transaction { get; private set; }




    }
}
