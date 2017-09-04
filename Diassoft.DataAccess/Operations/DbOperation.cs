using Diassoft.DataAccess.DatabaseObjects;
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
        /// Reference to the Database Connection in use
        /// </summary>
        public IDbConnection Connection { get; private set; }

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
        /// Reference to the Database Transaction in use
        /// </summary>
        public IDbTransaction Transaction { get; private set; }

        #endregion Properties

        #region Validation Methods

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

        /// <summary>
        /// Validate Conditions to Start Building any Statement 
        /// </summary>
        /// <exception cref="NullReferenceException">The exception that is thrown when there is no <see cref="Dialect"/> setup</exception>
        protected virtual void PreStatementValidation()
        {
            // Validate if Dialect was informed
            if (Dialect == null) throw new NullReferenceException($"There is no {nameof(Dialect)} setup");
        }


        #endregion Validation Methods

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
