using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    /// <summary>
    /// Interface to be implemented by classes that exposes a list value field
    /// </summary>
    /// <typeparam name="T">The <see cref="System.Type"/> of the values inside the list</typeparam>
    public sealed class IListValued<T>
    {
        /// <summary>
        /// Represents the List of Values
        /// </summary>
        IList<T> Values { get; set; }
    }
}
