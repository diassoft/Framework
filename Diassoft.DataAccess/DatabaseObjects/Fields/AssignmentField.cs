using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.DataAccess.DatabaseObjects.Fields
{
    /// <summary>
    /// A field to be assigned
    /// </summary>
    public class AssignmentField: Field
    {
        public object Value { get; set; }
    }
}
