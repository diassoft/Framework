using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    /// <summary>
    /// Interface to be implemented by classes that exposes a value field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValued<T>
    {
        /// <summary>
        /// The Value to be exposed
        /// </summary>
        T Value { get; set; }
    }
}
