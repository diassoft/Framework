using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding.Parsers
{
    /// <summary>
    /// The Interface to be implemented by Command Parsers
    /// </summary>
    public interface ICommandParser
    {
        /// <summary>
        /// Translates an input command into a undestandable object
        /// </summary>
        /// <param name="inputCommand">The Input Command</param>
        /// <returns>A parsed command</returns>
        ParsedCommand Parse(string inputCommand);

    }

    /// <summary>
    /// Represents a command parsed from an input command
    /// </summary>
    public class ParsedCommand
    {
        /// <summary>
        /// The command name
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// The arguments found for the command
        /// </summary>
        public ParsedCommandArgument[] Arguments { get; set; } = new ParsedCommandArgument[] { };
        /// <summary>
        /// Defines whether the user is requesting help information for the command
        /// </summary>
        /// <remarks>Usually when the second argument is a Question Mark, this is a help request</remarks>
        public bool IsHelpRequest { get; set; } = false;
        /// <summary>
        /// Defines whether the <see cref="Arguments">command arguments array</see> is configured by parameter name, parameter identifier. If False, the system will apply the arguments in sequence.
        /// </summary>
        public bool IsArgumentParameterSpecific { get; set; } = false;

        /// <summary>
        /// Converts the Parsed Command object to a string containing the Command and its Arguments
        /// </summary>
        /// <returns>A string representing the object</returns>
        public override string ToString()
        {
            var argumentsString = (Arguments == null ? "0" : String.Join<ParsedCommandArgument>(",", Arguments) );
            return $"Command: {Command}; Arguments: [{argumentsString}]";
        }
    }

    /// <summary>
    /// Represents a command argument
    /// </summary>
    public class ParsedCommandArgument
    {
        /// <summary>
        /// A unique identifier for the argument.
        /// </summary>
        /// <remarks>Use the identifier together with an attribute</remarks>
        public string Identifier { get; set; }
        /// <summary>
        /// The parameter name for the method
        /// </summary>
        public string Parameter { get; set; }
        /// <summary>
        /// The value to be passed to the parameter
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedCommandArgument"/>
        /// </summary>
        public ParsedCommandArgument(): this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedCommandArgument"/>
        /// </summary>
        /// <param name="value">The value of the parameter</param>
        public ParsedCommandArgument(string value): this(null, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedCommandArgument"/>
        /// </summary>
        /// <param name="parameter">The parameter name</param>
        /// <param name="value">The value of the parameter</param>
        public ParsedCommandArgument(string parameter, string value): this(null, parameter, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedCommandArgument"/>
        /// </summary>
        /// <param name="identifier">A unique identifier for the argument</param>
        /// <param name="parameter">The parameter name</param>
        /// <param name="value">The value of the parameter</param>
        public ParsedCommandArgument(string identifier, string parameter, string value)
        {
            Identifier = identifier;
            Parameter = parameter;
            Value = value;
        }

        /// <summary>
        /// Converts the Command Argument to a valid string format
        /// </summary>
        /// <returns>A string containing the Parsed Comand Argument</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Parameter))
                return Value;
            else
                return String.Format("{0}:{1}", Parameter, Value);
        }
    }
}
