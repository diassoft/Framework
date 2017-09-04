using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.DataAccess.DatabaseObjects.Fields;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    /// <summary>
    /// Represents a Filter Expression (x = y? x != y? x > y?)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FilterExpression<T> : Expression
    {
        #region IListValued<ILIst<T>> Implementation

        /// <summary>
        /// The list of values to be included on the Filter Expression
        /// </summary>
        public IList<T> Values { get; set; } = new List<T>();

        #endregion IValued<ILIst<T>> Implementation

        #region Constructors

        /// <summary>
        /// Initializes a new instance of an <see cref="AssignExpression{T}"/>.
        /// </summary>
        public FilterExpression(): base(FieldOperators.Equal)
        {

        }

        /// <summary>
        /// Initializes a new instance of an <see cref="AssignExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        public FilterExpression(Field field): base(field, FieldOperators.Equal)
        {
            Field = field;
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="AssignExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        /// <param name="value">The Value to be assigned to the field</param>
        public FilterExpression(Field field, T value) : this(field)
        {
            Values.Add(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="values"></param>
        public FilterExpression(Field field, IList<T> values) : this(field)
        {
            
        }

        #endregion Constructors
    }
}
