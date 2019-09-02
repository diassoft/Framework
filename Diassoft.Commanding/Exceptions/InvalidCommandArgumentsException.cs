using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an exception that is thrown when the arguments from the command could not be input for the command specification
    /// </summary>
    public class InvalidCommandArgumentsException: CommandBaseException
    {
        /// <summary>
        /// The default error message
        /// </summary>
        internal static string DEFAULTMESSAGE = "Unable to assign '{0}' ({1}) to parameter '{2}' ({3})";

        /// <summary>
        /// The parameter that could not be assigned
        /// </summary>
        public string ParameterName { get; }
        /// <summary>
        /// Type of the parameter that could not be assigned
        /// </summary>
        public Type ParameterType { get; }
        /// <summary>
        /// The value to be assigned to the parameter
        /// </summary>
        public string InputArgument { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandArgumentsException"/>
        /// </summary>
        /// <param name="parameterName">The parameter that could not be assigned</param>
        /// <param name="parameterType">The type of the parameter that could not be assigned</param>
        /// <param name="inputArgument">The value to be assigned to the parameter</param>
        public InvalidCommandArgumentsException(string command, string parameterName, Type parameterType, string inputArgument): this(command, parameterName, parameterType, inputArgument, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandArgumentsException"/>
        /// </summary>
        /// <param name="parameterName">The parameter that could not be assigned</param>
        /// <param name="parameterType">The type of the parameter that could not be assigned</param>
        /// <param name="inputArgument">The value to be assigned to the parameter</param>
        /// <param name="innerException">The exception that caused this exception to happen</param>
        public InvalidCommandArgumentsException(string command, string parameterName, Type parameterType, string inputArgument, Exception innerException): base(command, String.Format(InvalidCommandArgumentsException.DEFAULTMESSAGE, parameterName, parameterType?.Name, inputArgument, inputArgument.GetType().Name), innerException)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
            InputArgument = inputArgument;
        }

    }
}
