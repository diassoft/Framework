using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    /// <summary>
    /// Interface to be implemented by classes that exposes a single value field
    /// </summary>
    /// <typeparam name="T">The <see cref="System.Type"/> of the Value</typeparam>
    public interface IValued<T>
    {
        /// <summary>
        /// The Value to be exposed
        /// </summary>
        T Value { get; set; }
    }
}
