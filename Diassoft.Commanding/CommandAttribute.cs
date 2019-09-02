using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an attribute to bind a method to a command. Prefer to use the <see cref="CommandExecutionResponse"/> as the return type of the methods implementing this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CommandAttribute: System.Attribute
    {
        private string _Command;
        /// <summary>
        /// String representing the command input
        /// </summary>
        public string Command
        {
            get { return _Command; }
            set
            {
                if (value == null)
                    _Command = value;
                else
                    _Command = value.ToUpper();
            }
        }
        
        /// <summary>
        /// Description of the Command
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/>
        /// </summary>
        /// <param name="command">String representing the command input</param>
        public CommandAttribute(string command): this(command, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/>
        /// </summary>
        /// <param name="command">String representing the command input</param>
        /// <param name="description">Description of the command</param>
        public CommandAttribute(string command, string description)
        {
            Command = command;
            Description = description;
        }

    }
}
