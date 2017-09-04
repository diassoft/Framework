using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// Represents the Base of Any Database Operation
    /// </summary>
    public abstract class DbOperation
    {
        
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


        #region Query Execution Methods / Functions

        /// <summary>
        /// Runs the NonQuery Operation
        /// </summary>
        /// <returns>The number of rows processed</returns>
        public virtual int Run()
        {
            // Variable to hold the query
            string Query = "";

            try
            {
                // Validate Connection
                if (Connection == null) throw new NullReferenceException($"There is no {nameof(Connection)} defined for this operation");
                if (Connection?.State != ConnectionState.Open) throw new Exception($"The connection is at '{Enum.GetName(typeof(ConnectionState), Connection.State)}' state and therefore it is not possible to process an operation.");

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
        /// <param name="rowsProcessed">Reference to an <see cref="int">integer</see> to display the number of rows counted</param>
        /// <returns>A <see cref="bool">boolean</see> to define whether the execution succeed or failed</returns>
        public bool TryRun(out int rowsProcessed)
        {
            try
            {
                rowsProcessed = Run();
                return true;
            }
            catch 
            {
                rowsProcessed = 0;
                return false;
            }
        }

        public virtual object RunScalar()
        {
            // Variable to hold the query
            string Query = "";

            try
            {
                // Validate Connection
                if (Connection == null) throw new NullReferenceException($"There is no {nameof(Connection)} defined for this operation");
                if (Connection?.State != ConnectionState.Open) throw new Exception($"The connection is at '{Enum.GetName(typeof(ConnectionState), Connection.State)}' state and therefore it is not possible to process an operation.");

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

        #endregion Query Execution Methods / Functions

    }
}
