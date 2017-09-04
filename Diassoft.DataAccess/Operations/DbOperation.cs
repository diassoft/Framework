using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// Represents the Base of Any Database Operation Class
    /// </summary>
    public abstract class DbOperation
    {

        #region Properties

        /// <summary>
        /// The <see cref="Diassoft.DataAccess.Dialect">Dialect</see> to use when formatting fields and values for a statement
        /// </summary>
        public Dialect Dialect { get; protected set; }

        /// <summary>
        /// The Table Owner or Library
        /// </summary>
        public string TableOwner { get; set; }

        /// <summary>
        /// The Table Name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Represents the alias to refer to the Table.
        /// An alias is usually necessary when you have the same table twice on an statement.
        /// </summary>
        public string TableAlias { get; set; }

        /// <summary>
        /// The Formatted Table Name, including the alias
        /// </summary>
        public string FullTableName
        {
            get
            {
                if (Dialect == null) throw new NullReferenceException($"A {nameof(Dialect)} has not been setup for the operation");

                return this.Dialect.FormatTable(TableOwner, TableName, TableAlias);
            }
        }

        /// <summary>
        /// Reference to the Database Connection in use
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// Reference to the Database Transaction in use
        /// </summary>
        public IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// Returns whether a Connection has been defined and if it's valid for running
        /// </summary>
        public bool IsConnectionValid
        {
            get
            {
                if (Connection == null) return false;
                if (Connection?.State != ConnectionState.Open) return false;

                return true;
            }
        }

        /// <summary>
        /// Validates the Database Connection
        /// </summary>
        public void ValidateConnection()
        {
            // Checks if the connection exists
            if (Connection == null) throw new NullReferenceException($"There is no {nameof(Connection)} defined for this operation");

            // Checks the connection state
            if (Connection?.State != ConnectionState.Open) throw new Exception($"The connection is at '{Enum.GetName(typeof(ConnectionState), Connection.State)}' state and therefore it is not possible to perform any operation.");
        }

        #endregion Properties

        #region Abstract Methods to be implemented by classes inheriting from this class

        /// <summary>
        /// Creates the SQL Statement
        /// </summary>
        /// <returns></returns>
        public abstract string GetStatement();

        #endregion Abstract Methods to be implemented by classes inheriting from this class

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Database Operation
        /// </summary>
        protected DbOperation()
        {
            // Initialize Table Variables
            TableOwner = "";
            TableName = "";
            TableAlias = "";
        }

        /// <summary>
        /// Initializes a new instance of the Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected DbOperation(Dialect dialect): this()
        {
            Dialect = dialect;
        }

        /// <summary>
        /// Initializes a new instance of the Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected DbOperation(Dialect dialect, IDbConnection connection): this(dialect)
        {
            Connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected DbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): this(dialect, connection)
        {
            Transaction = transaction;
        }

        #endregion Constructors


    }
}
