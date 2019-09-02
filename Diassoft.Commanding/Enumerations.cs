using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Defines the possible lifetimes of an command class
    /// </summary>
    public enum InstanceLifetimes : byte
    {
        /// <summary>
        /// Command Class is created once for the entire service lifetime
        /// </summary>
        Singleton = 0,
        /// <summary>
        /// Command Class is created once for a session
        /// </summary>
        Scoped = 1,
        /// <summary>
        /// Command Class is created each time the command is called
        /// </summary>
        Transient = 2
    }

    /// <summary>
    /// Defines the actions to be taken when a duplicated command is attempted to be registered
    /// </summary>
    /// <remarks>An event is also triggered when a duplicated command is found</remarks>
    public enum DuplicatedCommandActions: byte
    {
        /// <summary>
        /// Ignore duplicated command
        /// </summary>
        Ignore = 0,
        /// <summary>
        /// Replace current command with the new command
        /// </summary>
        Replace = 1,
        /// <summary>
        /// Raises an exception and stops processing
        /// </summary>
        Error = 9
    }

    /// <summary>
    /// Defines the valid status for a Command
    /// </summary>
    /// <remarks>This class contains the basic status return of a command. The <see cref="CommandExecutionResponse"/> object allow the return of custom status using the <see cref="CommandExecutionResponse.CustomStatusCode"/> property</remarks>
    public enum CommandExecutionResponseStatus: byte
    {
        /// <summary>
        /// The command was successfully executed
        /// </summary>
        Success = 0,
        /// <summary>
        /// The command should return help instructions
        /// </summary>
        Help = 1,
        /// <summary>
        /// The command had errors on its parameters, returning the help instructions
        /// </summary>
        HelpWithErrors = 253,
        /// <summary>
        /// The command could not be found
        /// </summary>
        CommandNotFound = 254,
        /// <summary>
        /// The command executed with errors
        /// </summary>
        Error = 255,
    }
}
