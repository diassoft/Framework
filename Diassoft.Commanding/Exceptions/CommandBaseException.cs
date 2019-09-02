using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents the base class of an exception related to a command execution
    /// </summary>
    public abstract class CommandBaseException: System.Exception
    {
        /// <summary>
        /// The command that could not be found
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBaseException"/>
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="command">The command related to the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        protected CommandBaseException(string message, string command, Exception innerException): base(message, innerException)
        {
            Command = command;
        }

    }
}
