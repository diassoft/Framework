using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.DataAccess.DatabaseObjects;
using System.Data;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// A class that represents a Select database operation
    /// </summary>
    public class SelectDbOperation: InquiryDbOperation
    {

        #region Constructors

        /* Since this is using a inheritance structure, it's necessary to redefine the methods on this base class.
         * It's going to call the Base Methods always, there is no need to implement any logic here */

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        protected SelectDbOperation() : base() { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        protected SelectDbOperation(Dialect dialect): base(dialect) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection): base(dialect, connection) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction): base(dialect, connection, transaction) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectDbOperation(Dialect dialect, Table table) : base(dialect, table) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, Table table) : base(dialect, connection, table) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="table">The <see cref="Table"/> to be inquired</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table table) : base(dialect, connection, transaction, table) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected SelectDbOperation(Dialect dialect, params Table[] tables) : base(dialect, tables) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, params Table[] tables) : base(dialect, connection, tables) { }

        /// <summary>
        /// Initializes a new instance of the Select Database Operation
        /// </summary>
        /// <param name="dialect">The <see cref="DataAccess.Dialect">Dialect</see> to communicate with the database</param>
        /// <param name="connection">Reference to the <see cref="System.Data.IDbConnection">Connection</see> to the database</param>
        /// <param name="transaction">Reference to the <see cref="System.Data.IDbTransaction">Transaction</see> in use for the given connection</param>
        /// <param name="tables">An array of <see cref="Table"/> with the tables to be searched</param>
        protected SelectDbOperation(Dialect dialect, IDbConnection connection, IDbTransaction transaction, Table[] tables) : base(dialect, connection, transaction, tables) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Defines whether the select will be of the distinct fields.
        /// By default the value is false.
        /// </summary>
        public bool Distinct { get; set; } = false;

        /// <summary>
        /// Defines the Top N Rows to be displayed.
        /// By default the value is zero.
        /// </summary>
        public int TopNRows { get; set; } = 0;


        #endregion Properties

        #region Methods / Functions

        /// <summary>
        /// Returns the Statement for the <see cref="SelectDbOperation"/>.
        /// </summary>
        /// <returns>A string containing the select statement</returns>
        public override string GetStatement()
        {
            throw new NotImplementedException();
        }

        #endregion Methods / Functions


    }
}
