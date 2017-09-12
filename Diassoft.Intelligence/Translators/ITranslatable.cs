using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Intelligence.Translators
{
    /// <summary>
    /// Defines an interface that all Translators must implement
    /// </summary>
    /// <typeparam name="TOrigin">The Origin Type (content to be translated)</typeparam>
    /// <typeparam name="TDestination">The Destination Type (translation)</typeparam>
    public interface ITranslatable<TOrigin, TDestination>
    {
        /// <summary>
        /// A function that will perform the translation
        /// </summary>
        /// <param name="input">The input parameter</param>
        /// <returns>Returns the transaction</returns>
        TDestination Translate(TOrigin input);

        /// <summary>
        /// Tries to translate a value
        /// </summary>
        /// <param name="input">The input parameter</param>
        /// <param name="translation">The translation</param>
        /// <returns>A <see cref="bool"/> value to define whether the translation was successfull or not</returns>
        bool TryTranslate(TOrigin input, out TDestination translation);
    }
}
