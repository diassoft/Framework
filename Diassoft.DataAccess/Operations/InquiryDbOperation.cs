using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Diassoft.DataAccess.DatabaseObjects;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// Represents the Base Class for any Database Operation that read records
    /// </summary>
    public abstract class InquiryDbOperation: DbOperation
    {

        #region Properties

        /// <summary>
        /// A list containing all the tables being inquired on the operation
        /// </summary>
        /// <remarks>The order the tables are added is the order they will be placed on the inquiry statement</remarks>
        public List<Table> Tables { get; set; } = new List<Table>();

        #endregion Properties
        
        #region Reader Functions

        /// <summary>
        /// Runs the Inquiry Statement and output results to a Data Reader using the default command behavior
        /// </summary>
        /// <remarks>Please note that when using <see cref="IDataReader">DataReaders</see>, the system will only allow one query to run at a time.</remarks>
        /// <returns>A <see cref="System.Data.IDataReader"/> with the data.</returns>
        public virtual IDataReader RunReader()
        {
            return RunReader(CommandBehavior.Default);
        }

        /// <summary>
        /// Runs the Inquiry Statement and output results to a Data Reader
        /// </summary>
        /// <param name="behavior">A <see cref="CommandBehavior"/> to define the isolation level of the query execution.</param>
        /// <remarks>Please note that when using <see cref="IDataReader">DataReaders</see>, the system will only allow one query to run at a time.</remarks>
        /// <returns>A <see cref="System.Data.IDataReader"/> with the data.</returns>
        public virtual IDataReader RunReader(CommandBehavior behavior)
        {
            // Make sure the connection is valid (this will throw exceptions when the connection is invalid)
            ValidateConnection();

            // Create the needed objects to run a reader query
            IDbCommand cmd = base.Connection.CreateCommand();
            cmd.Transaction = Transaction;
            cmd.CommandText = GetStatement();
            cmd.CommandType = CommandType.Text;

            return cmd.ExecuteReader(behavior);
        }

        /// <summary>
        /// Runs the Inquiry Statement and output results to a Data Reader
        /// </summary>
        /// <param name="behavior">A <see cref="CommandBehavior"/> to define the isolation level of the query execution.</param>
        /// <param name="reader">Reference to a <see cref="IDataReader"/> where the data will be sent.</param>
        /// <remarks>Please note that when using <see cref="IDataReader">DataReaders</see>, the system will only allow one query to run at a time.</remarks>
        /// <returns>A <see cref="bool"/> to define whether the Inquiry Statement succeeded or failed</returns>
        public bool TryRunReader(CommandBehavior behavior, out IDataReader reader)
        {
            try
            {
                reader = RunReader(behavior);
                return true;
            }
            catch
            {
                reader = null;
                return false;
            }
        }
        
        #endregion Reader Functions

        #region Scalar Functions

        /// <summary>
        /// Runs a query that should return only a single row and column
        /// </summary>
        /// <returns>An <see cref="object">object</see> containing the results of the statement</returns>
        public virtual object RunScalar()
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
                    return command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion Scalar Functions

        //TODO: Implement Run JSON
        //TODO: Implement Run DataTable
        //TODO: Implement Run XML

        #region Constructors

        /* Since this is using a inheritance structure, it's necessary to redefine the methods on this base class.
         * It's going to call the Base Methods always, there is no need to implement any logic here */

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        protected InquiryDbOperation() : base() { }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected InquiryDbOperation(Dialect dialect): base(dialect) { }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected InquiryDbOperation(Dialect dialect, IDbConnection connection): base(dialect, connection) { }
        
        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected InquiryDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): base(dialect, connection, transaction) { }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected InquiryDbOperation(Dialect dialect, Table table) : this(dialect) { Tables.Add(table); }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected InquiryDbOperation(Dialect dialect, IDbConnection connection, Table table) : this(dialect, connection) { Tables.Add(table); }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected InquiryDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table table) : this(dialect, connection, transaction) { Tables.Add(table); }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected InquiryDbOperation(Dialect dialect, params Table[] tables) : this(dialect) { Tables.AddRange(tables); }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected InquiryDbOperation(Dialect dialect, IDbConnection connection, params Table[] tables) : this(dialect, connection) { Tables.AddRange(tables); }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected InquiryDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table[] tables) : this(dialect, connection, transaction) { Tables.AddRange(tables); }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Validates the Conditions for the Statement Execution
        /// </summary>
        public new void PreStatementValidation()
        {
            // Call the default PreStatementValidation
            base.PreStatementValidation();

            // Validate number of Tables
            if (Tables?.Count == 0) throw new Exception($"You must define at least one table on the {nameof(InquiryDbOperation)}.");
        }

        #endregion Methods

    }
}
