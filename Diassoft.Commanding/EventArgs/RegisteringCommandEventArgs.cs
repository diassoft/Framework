using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents the Event Arguments for the Command Registering event
    /// </summary>
    public class RegisteringCommandEventArgs: System.EventArgs
    {
        /// <summary>
        /// Name of the Command being registered
        /// </summary>
        public string CommandName { get; set; }
        /// <summary>
        /// Defines whether to suppress the registration or not. By Default, the registration is allowed to continue.
        /// </summary>
        public bool SuppressRegistration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteringCommandEventArgs"/>
        /// </summary>
        internal RegisteringCommandEventArgs(): this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteringCommandEventArgs"/>
        /// </summary>
        /// <param name="commandName">The command being registered</param>
        internal RegisteringCommandEventArgs(string commandName): this(commandName, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteringCommandEventArgs"/>
        /// </summary>
        /// <param name="commandName">The command being registered</param>
        /// <param name="suppressRegistration">A flag to define whether the command registration should be suppressed</param>
        internal RegisteringCommandEventArgs(string commandName, bool suppressRegistration)
        {
            CommandName = commandName;
            SuppressRegistration = suppressRegistration;
        }
    }
}
