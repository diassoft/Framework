using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.DataAccess.DatabaseObjects.Fields;

namespace Diassoft.DataAccess.DatabaseObjects.Expressions
{
    public abstract class Expression
    {

        public Field Field { get; set; }

        public FieldOperators Operator { get; set; }



    }
}
