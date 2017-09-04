using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.Operations
{
    /// <summary>
    /// An interface to be implemented for classes that have a Keyed Operation
    /// </summary>
    public interface IKeyedDbOperation
    {
        List<string> Keys { get; set; }
    }
}
