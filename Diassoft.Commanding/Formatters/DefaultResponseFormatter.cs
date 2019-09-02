using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding.Formatters
{
    /// <summary>
    /// Represents the default formatter for a command execution response
    /// </summary>
    public sealed class DefaultResponseFormatter : IResponseFormatter
    {
        /// <summary>
        /// Formats the response into a valid string
        /// </summary>
        /// <param name="response">The response to be formatted</param>
        /// <returns>A string containing the formatted command response</returns>
        public string Format(CommandExecutionResponse response)
        {
            if (response.Status == CommandExecutionResponseStatus.Success)
            {
                // Success Message

                // Return the command response if there is any, if not, just return a success message
                if ((response.Data != null) && (response.Data?.ToString() != ""))
                    return response.Data.ToString();
                else
                    return String.Format("{0} SUCCESS", response.CustomStatusCode.ToString());

            }
            else if (response.Status == CommandExecutionResponseStatus.Error)
            {
                // Error Message

                // Display the exceptions or just error
                if (response.Exceptions?.Count > 0)
                {
                    var exception = response.Exceptions.Pop();
                    if (response.CustomStatusCode > 0)
                        return String.Format("{0} ERROR - {1}",
                                             response.CustomStatusCode.ToString(),
                                             exception.Message);

                    return String.Format("ERROR - {0}", exception.Message);
                }
                else
                {
                    if (response.CustomStatusCode > 0)
                        return String.Format("{0} ERROR", response.CustomStatusCode.ToString());

                    return "ERROR";
                }
            }
            else if (response.Status == CommandExecutionResponseStatus.CommandNotFound)
            {
                return "ERROR - Invalid command";
            }
            else if (response.Status == CommandExecutionResponseStatus.HelpWithErrors)
            {
                // Help Request + Error

                // Display the Error and then the command help
                if (response.Exceptions?.Count > 0)
                {
                    var exception = response.Exceptions.Pop();
                    return String.Format("{0} ERROR - {1}{2}",
                                         response.CustomStatusCode.ToString(),
                                         exception.Message,
                                         String.IsNullOrEmpty(response.Data?.ToString()) ? "" : $"\n\n{response.Data.ToString()}");
                }
                else
                    return String.Format("{0} ERROR{1}", 
                                         response.CustomStatusCode.ToString(),
                                         String.IsNullOrEmpty(response.Data?.ToString()) ? "" : $"\n\n{response.Data.ToString()}");
            }
            else
            {
                return response.Data?.ToString();
            }

        }
    }
}
