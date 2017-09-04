using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Diassoft.DataAccess.DatabaseObjects;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// Represents the Base Class for any Database Operation that change records
    /// </summary>
    public abstract class ModifyDbOperation: DbOperation
    {
        #region Properties

        /// <summary>
        /// The Table to be Modified
        /// </summary>
        public Table Table { get; set; }

        /// <summary>
        /// The Formatted Table Name, including the alias
        /// </summary>
        /// <exception cref="NullReferenceException">An exception that is thrown when there is no <see cref="Dialect"/> or <see cref="Table"/> setup.</exception>
        public string FullTableName
        {
            get
            {
                if (Dialect == null) throw new NullReferenceException($"A {nameof(Dialect)} has not been setup for the operation");
                if (Table == null) throw new NullReferenceException($"A {nameof(Table)} has not been defined for the operation");

                return this.Dialect.FormatTable(Table);
            }
        }

        #endregion Properties

        #region Query Execution Methods

        /// <summary>
        /// Runs the NonQuery Operation
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public virtual int Run()
        {
            // Variable to hold the query
            string Query = "";

            try
            {
                // Validate Connection
                ValidateConnection();

                // Gets the Query based on the GetStatement Function
                Query = GetStatement();

                using (IDbCommand command = Connection.CreateCommand())
                {
                    if (Transaction != null) command.Transaction = Transaction;
                    command.CommandText = Query;
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Tries to run the operation
        /// </summary>
        /// <param name="rowsAffected">Reference to an <see cref="int">integer</see> to display the number of rows counted</param>
        /// <returns>A <see cref="bool">boolean</see> to define whether the execution succeed or failed</returns>
        public bool TryRun(out int rowsAffected)
        {
            try
            {
                rowsAffected = Run();
                return true;
            }
            catch
            {
                rowsAffected = 0;
                return false;
            }
        }

        #endregion Query Execution Methods

        #region Constructors

        /* Since this is using a inheritance structure, it's necessary to redefine the methods on this base class.
         * It's going to call the Base Methods always, there is no need to implement any logic here */

        /// <summary>
        /// Initializes a new instance of the Modify Database Operation
        /// </summary>
        protected ModifyDbOperation() : base() { }

        /// <summary>
        /// Initializes a new instance of the Modify Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected ModifyDbOperation(Dialect dialect): base(dialect) { }

        /// <summary>
        /// Initializes a new instance of the Modify Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected ModifyDbOperation(Dialect dialect, IDbConnection connection): base(dialect, connection) { }

        /// <summary>
        /// Initializes a new instance of the Modify Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected ModifyDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): base(dialect, connection, transaction) { }

        #endregion Constructors

    }
}
