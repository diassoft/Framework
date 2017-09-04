using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.DatabaseObjects.Fields
{
    public enum FieldTypes : int
    {
        None = 0,
        String = 1,
        Numeric = 2,
        Decimal = 3,
        DateTime = 4,
        Boolean = 5,
        /// <summary>
        /// Represents a field to be displayed in a select statement.
        /// </summary>
        Display = 90,
        /// <summary>
        /// Represents an aggregate field on an Sql Statement.
        /// </summary>
        Aggregate = 91,
        /// <summary>
        /// Represents a field to be filtered according to another field.
        /// </summary>
        FieldFilter = 92
    }

    public enum FieldOperators : int
    {
        None = 0,
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        LessThan = 4,
        GreaterThanOrEqual = 5,
        LessThanOrEqual = 6,
        Like = 7,
        NotLike = 8,
        In = 9,
        NotIn = 10,
        IsNull = 11,
        NotIsNull = 12,
        Between = 13
    }

    public enum FieldAndOr : int
    {
        None = 0,
        And = 1,
        Or = 2
    }

    public enum Aggregates : int
    {
        Count = 0,
        Max = 1,
        Min = 2,
        Average = 3,
        CountDistinct = 4,
        Sum = 5
    }

}
