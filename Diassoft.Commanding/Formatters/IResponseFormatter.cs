using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding.Formatters
{
    /// <summary>
    /// Defines an interface to be implmented by any response formatter
    /// </summary>
    public interface IResponseFormatter
    {
        /// <summary>
        /// Method to format the response into a valid text
        /// </summary>
        /// <param name="response">The Command Execution Response</param>
        /// <returns>A string containing the formatted response</returns>
        string Format(CommandExecutionResponse response);
    }
}
