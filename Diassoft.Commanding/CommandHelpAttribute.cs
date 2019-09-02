using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an attribute containing the Command Help in text format
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CommandHelpAttribute: System.Attribute
    {
        /// <summary>
        /// A code that represents the Culture Name. Usually the language followed by the country, but language only is also accepted.
        /// </summary>
        /// <remarks>
        /// Language follows the <see cref="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO 639-1 codes</see>.
        /// Country follows the <see cref="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha 2 codes</see>.</remarks>
        public string CultureName { get; private set; }
        /// <summary>
        /// The contents
        /// </summary>
        public string Contents { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHelpAttribute"/>
        /// </summary>
        /// <param name="contents">The contents of the help request</param>
        public CommandHelpAttribute(string contents): this(null, contents) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHelpAttribute"/>
        /// </summary>
        /// <param name="cultureName">A string representing the culture name. Usually the two digit language as defined per <see cref="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO 639-1 codes</see> and a two digit country as defined per <see cref="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha 2 codes</see></param>
        /// <param name="contents">The contents of the help request</param>
        public CommandHelpAttribute(string cultureName, string contents)
        {
            CultureName = cultureName;
            Contents = contents;
        }

    }
}
