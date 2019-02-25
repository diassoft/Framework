using Diassoft.DataAccess.DatabaseObjects;
using Diassoft.DataAccess.DatabaseObjects.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// A class that represents an Update database operation
    /// </summary>
    public class UpdateDbOperation: DbOperation<Table>
    {
        /// <summary>
        /// The Assignments
        /// </summary>
        public AssignExpression[] Assignments { get; set; }

        /// <summary>
        /// The filters for the assignment
        /// </summary>
        public object[] Where { get; set; }

        /// <summary>
        /// Initializes a new <see cref="UpdateDbOperation"/>
        /// </summary>
        public UpdateDbOperation() : this((Table)null) { }

        /// <summary>
        /// Initializes a new <see cref="UpdateDbOperation"/>
        /// </summary>
        /// <param name="tableName">A <see cref="string"/> representing the object to be modified</param>
        public UpdateDbOperation(string tableName) : this(new Table(tableName)) { }

        /// <summary>
        /// Initializes a new <see cref="UpdateDbOperation"/>
        /// </summary>
        /// <param name="table">A <see cref="Table"/> representing the object to be modified</param>
        public UpdateDbOperation(Table table)
        {
            base.Table = table;

            // Always remove the alias on an insert operation
            base.Table.Alias = "";

        }
    }
}
