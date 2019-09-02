using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents the Event Arguments for the Duplicated Command Event
    /// </summary>
    public class DuplicatedCommandEventArgs: System.EventArgs
    {
        /// <summary>
        /// The command that was found to be duplicated
        /// </summary>
        public string CommandName { get; set; }
        /// <summary>
        /// Action to be taken for the duplicated command
        /// </summary>
        public DuplicatedCommandActions Action { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicatedCommandEventArgs"/>
        /// </summary>
        /// <param name="commandName">The command that was found to be duplicated</param>
        /// <param name="action">Action to be taken for the duplicated command</param>
        internal DuplicatedCommandEventArgs(string commandName, DuplicatedCommandActions action)
        {
            this.CommandName = commandName;
            this.Action = action;
        }
    }
}
