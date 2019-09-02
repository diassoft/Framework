using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an exception thrown when a command is not found
    /// </summary>
    public class CommandNotFoundException: CommandBaseException
    {
        /// <summary>
        /// The default error message
        /// </summary>
        internal static string DEFAULTMESSAGE = "The command '{0}' could not be found";

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandNotFoundException"/> exception
        /// </summary>
        /// <param name="command">The command that could not be found</param>
        public CommandNotFoundException(string command) : this(command, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandNotFoundException"/> exception
        /// </summary>
        /// <param name="command">The command that could not be found</param>
        /// <param name="innerException">The exception that caused this exception</param>
        public CommandNotFoundException(string command, Exception innerException): base(command, string.Format(CommandNotFoundException.DEFAULTMESSAGE, command), innerException) { }

    }
}
