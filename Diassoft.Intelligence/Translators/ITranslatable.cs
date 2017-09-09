using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Intelligence.Translators
{
    public interface ITranslatable<TOrigin, TDestination>
    {
        TDestination Translate(TOrigin input);

        bool TryTranslate(TOrigin input, out TDestination translation);
    }
}
