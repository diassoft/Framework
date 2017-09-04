using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.DataAccess.DatabaseObjects.Fields;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    /// <summary>
    /// Represents the base class for an Expression. Assignments and Filters are considered expressions. (x = y)
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// The Field to be Used
        /// </summary>
        public Field Field { get; set; }

        /// <summary>
        /// The <see cref="FieldOperators">FieldOperator</see>
        /// </summary>
        public FieldOperators Operator { get; set; }

        /// <summary>
        /// Initializes a new expression
        /// </summary>
        protected Expression()
        {

        }

        /// <summary>
        /// Initializes a new expression
        /// </summary>
        /// <param name="field">The Field to be Processed by the <see cref="Expression"/>.</param>
        protected Expression(Field field): this()
        {
            Field = field;
        }

        /// <summary>
        /// Initializes a new expression
        /// </summary>
        /// <param name="operator">The Operator. See <see cref="FieldOperators"/> for reference</param>
        protected Expression(FieldOperators @operator) : this()
        {
            Operator = @operator;
        }

        /// <summary>
        /// Initializes a new expression
        /// </summary>
        /// <param name="field">The Field to be Processed by the <see cref="Expression"/>.</param>
        /// <param name="operator">The Operator. See <see cref="FieldOperators"/> for reference</param>
        protected Expression(Field field, FieldOperators @operator): this(field)
        {
            Operator = @operator;
        }

    }
}
