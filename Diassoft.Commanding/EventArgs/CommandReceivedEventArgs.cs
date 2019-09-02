using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents the Event Arguments for the Command Received Event
    /// </summary>
    public sealed class CommandReceivedEventArgs: System.EventArgs
    {
        /// <summary>
        /// The Command Received
        /// </summary>
        public string Command { get; }
        /// <summary>
        /// The Session where the command was received
        /// </summary>
        public string SessionID { get; set; }
        /// <summary>
        /// The Response to the Command
        /// </summary>
        public CommandExecutionResponse Response { get; set; }
        /// <summary>
        /// Exception raised during command execution
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Initializes a new instance of the Command Received Event Arguments
        /// </summary>
        /// <param name="command">The Command Received</param>
        /// <param name="sessionID">The Session ID</param>
        public CommandReceivedEventArgs(string command, string sessionID)
        {
            this.Command = command;
            this.SessionID = sessionID;
        }

    }
}
