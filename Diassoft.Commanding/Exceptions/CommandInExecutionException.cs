using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an exception that is thrown when the command failed to execute
    /// </summary>
    public class CommandInExecutionException: CommandBaseException
    {
        /// <summary>
        /// The default error message
        /// </summary>
        internal static string DEFAULTMESSAGE = "There was an error during the execution of the '{0}' command";

        /// <summary>
        /// The command arguments
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInExecutionException"/> exception
        /// </summary>
        /// <param name="command">The command that failed to execute</param>
        public CommandInExecutionException(string command, object[] args) : this(command, args, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInExecutionException"/> exception
        /// </summary>
        /// <param name="command">The command that failed to execute</param>
        /// <param name="args">An array of objects containing the command arguments</param>
        /// <param name="innerException">The exception that caused this exception</param>
        public CommandInExecutionException(string command, object[] args, Exception innerException) : base(command, string.Format(CommandNotFoundException.DEFAULTMESSAGE, command), innerException)
        {
            Arguments = args;
        }

    }
}
