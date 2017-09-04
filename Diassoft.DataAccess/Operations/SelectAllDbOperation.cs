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
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        protected SelectAllDbOperation() : base() { }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected SelectAllDbOperation(Dialect dialect): base(dialect) { }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected SelectAllDbOperation(Dialect dialect, IDbConnection connection): base(dialect, connection) { }

        /// <summary>
        /// Initializes a new instance of the Inquiry Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected SelectAllDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): base(dialect, connection, transaction) { }

        #endregion Constructors

        #region Methods / Functions

        /// <summary>
        /// Returns the Select All Statement
        /// </summary>
        /// <returns></returns>
        public override string GetStatement()
        {
            throw new NotImplementedException();
        }

        #endregion Methods / Functions
    }
}
