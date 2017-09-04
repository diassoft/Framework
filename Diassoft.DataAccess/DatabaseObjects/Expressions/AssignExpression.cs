using Diassoft.DataAccess.DatabaseObjects.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    /// <summary>
    /// Represents an Assignment Expression (x = y)
    /// </summary>
    /// <typeparam name="T">The Type of the Value Field</typeparam>
    public sealed class AssignExpression<T> : Expression, IValued<T>
    {

        #region IValued<T> Implementation

        /// <summary>
        /// The value to be assigned to the expression
        /// </summary>
        public T Value { get; set; }

        #endregion IValued<T> Implementation

        #region Constructors

        /// <summary>
        /// Initializes a new instance of an <see cref="AssignExpression{T}"/>.
        /// </summary>
        public AssignExpression(): base(FieldOperators.Equal)
        {

        }

        /// <summary>
        /// Initializes a new instance of an <see cref="AssignExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        public AssignExpression(Field field): base(field, FieldOperators.Equal)
        {
            Field = field;
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="AssignExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        /// <param name="value">The Value to be assigned to the field</param>
        public AssignExpression(Field field, T value) : this(field)
        {
            Value = value;
        }

        #endregion Constructors

    }
}
