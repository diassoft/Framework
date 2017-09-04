using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Diassoft.DataAccess.DatabaseObjects;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// A class that represents a Select All database operation
    /// </summary>
    public class SelectAllDbOperation: InquiryDbOperation
    {

        #region Constructors

        /* Since this is using a inheritance structure, it's necessary to redefine the methods on this base class.
         * It's going to call the Base Methods always, there is no need to implement any logic here */

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        protected SelectAllDbOperation() : base() { }

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected SelectAllDbOperation(Dialect dialect): base(dialect) { }

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected SelectAllDbOperation(Dialect dialect, IDbConnection connection): base(dialect, connection) { }

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected SelectAllDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): base(dialect, connection, transaction) { }

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectAllDbOperation(Dialect dialect, Table table) : base(dialect, table) { }

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectAllDbOperation(Dialect dialect, IDbConnection connection, Table table) : base(dialect, connection, table) { }

        /// <summary>
        /// Initializes a new instance of the Select All Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectAllDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table table) : base(dialect, connection, transaction, table) { }

        #endregion Constructors

        #region Methods / Functions

        /// <summary>
        /// Validates the Conditions for the Statement Execution
        /// </summary>
        public new void PreStatementValidation()
        {
            // Call the default PreStatementValidation
            base.PreStatementValidation();

            // Validate number of Tables
            if (base.Tables?.Count == 0) throw new Exception($"You must define at least one table on the {nameof(SelectAllDbOperation)}.");
            else if (base.Tables?.Count > 1) throw new Exception($"The {nameof(SelectAllDbOperation)} does not support multiple tables on the statement. Currently there are {base.Tables.Count.ToString()} tables to be searched.");
        }

        /// <summary>
        /// Returns the Select All Statement
        /// </summary>
        /// <returns></returns>
        public override string GetStatement()
        {
            // Perform Necessary Validations
            this.PreStatementValidation();
            
            return base.Dialect.FormatStatement($"SELECT * FROM {base.Dialect.FormatTable(base.Tables[0])}");
         
        }

        #endregion Methods / Functions

    }
}
