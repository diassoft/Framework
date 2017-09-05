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
    public sealed class FilterExpression<T> : Expression, IListValued<T>
    {
        #region IListValued<T> Implementation

        /// <summary>
        /// The list of values to be included on the Filter Expression
        /// </summary>
        public IList<T> Values { get; set; } = new List<T>();

        #endregion IValued<ILIst<T>> Implementation

        #region Constructors

        /// <summary>
        /// Initializes a new instance of an <see cref="FilterExpression{T}"/>.
        /// </summary>
        public FilterExpression(): base(FieldOperators.Equal)
        {

        }

        /// <summary>
        /// Initializes a new instance of an <see cref="FilterExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        /// <param name="operator">The Operator. See <see cref="FieldOperators"/> for reference</param>
        public FilterExpression(Field field, FieldOperators @operator): base(field, @operator)
        {
            Field = field;
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="FilterExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        /// <param name="value">The Value to be assigned to the field</param>
        /// <param name="operator">The Operator. See <see cref="FieldOperators"/> for reference</param>
        public FilterExpression(Field field, FieldOperators @operator, T value) : this(field, @operator)
        {
            Values.Add(value);
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="FilterExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        /// <param name="operator">The Operator. See <see cref="FieldOperators"/> for reference</param>
        /// <param name="values">A <see cref="IList{T}"/> of values</param>
        public FilterExpression(Field field, FieldOperators @operator, IList<T> values) : this(field, @operator)
        {
            Values = values;
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="FilterExpression{T}"/>.
        /// </summary>
        /// <param name="field">A <see cref="Field"/> to be processed by the expression</param>
        /// <param name="operator">The Operator. See <see cref="FieldOperators"/> for reference</param>
        /// <param name="values">A <see cref="IList{T}"/> of values</param>
        public FilterExpression(Field field, FieldOperators @operator, params T[] values) : this(field, @operator)
        {
            Values = values;
        }

        /// <summary>
        /// Process the Operator accordingly to the number of elements on the List
        /// </summary>
        internal void ParseOperator()
        {
            if (Values == null) return;

            if (Values.Count == 0) return;

            if (Values.Count == 1)
            {
                if (base.Operator == FieldOperators.In) base.Operator = FieldOperators.Equal;
                else if (base.Operator == FieldOperators.NotIn) base.Operator = FieldOperators.NotEqual;

            }
        }

        #endregion Constructors
    }
}
