using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Diassoft.Intelligence.Translators
{
    /// <summary>
    /// This class receives an string input and translates it to a valid date
    /// </summary>
    public class StringToDateTranslator : ITranslatable<string, DateTime>
    {
        public DateTime Translate(string input)
        {
            throw new NotImplementedException();
        }

        public bool TryTranslate(string input, out DateTime translation)
        {
            throw new NotImplementedException();
        }

    }
}
