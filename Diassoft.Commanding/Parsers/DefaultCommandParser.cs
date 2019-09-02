using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding.Parsers
{
    /// <summary>
    /// Parse commands separated by spaces and enclosed in double quotes
    /// </summary>
    /// <remarks>Enclosing the sentences in quotes is optional</remarks>
    public class DefaultCommandParser : ICommandParser
    {
        /// <summary>
        /// Parses the Input into valid Command Syntax
        /// </summary>
        /// <param name="inputCommand">The input command</param>
        /// <returns>A <see cref="ParsedCommand"/> containing the processed input command</returns>
        public ParsedCommand Parse(string inputCommand)
        {
            // Each parameter separated from the command input
            var args = new List<ParsedCommandArgument>();

            // Default Values
            char delimiter = ' ';
            bool isQuoteOpen = false;

            // Allocate memory with the total command size
            StringBuilder sbCurrentData = new StringBuilder(inputCommand.Length);

            // Loop thru all characters on the command
            int iChar = 0;

            while (iChar < inputCommand.Length)
            {
                // Gets the current character
                var currentChar = inputCommand[iChar];

                // Start checking for quotes and double quotes
                if (currentChar == '"')
                {
                    // This is a quote

                    // Check if it's not a double quote, but ensure it's not the last digit already
                    if ((iChar + 1) < inputCommand.Length)
                    {
                        if (inputCommand[iChar + 1] == '"')
                        {
                            // This is a double quote, then just append one quote
                            sbCurrentData.Append(currentChar);

                            // Move to next char
                            iChar++;
                        }
                        else
                        {
                            // Not a double quote, so just switch the isQuoteOpen flag
                            isQuoteOpen = !isQuoteOpen;
                        }
                    }
                    else
                    {
                        // Last digit, close quote
                        isQuoteOpen = !isQuoteOpen;
                    }
                }
                else if (currentChar == delimiter)
                {
                    // This is the delimiter

                    // Quote is still open, just append to the current data then
                    if (isQuoteOpen)
                        sbCurrentData.Append(currentChar);
                    else
                    {
                        args.Add(new ParsedCommandArgument(sbCurrentData.ToString()));
                        sbCurrentData.Clear();
                    }
                }
                else
                {
                    // This is any random character
                    sbCurrentData.Append(currentChar);
                }

                // Next character
                iChar++;
            }

            // If there is still any leftover on the StringBuilder, it should be added to the List of Objects
            if (sbCurrentData.Length > 0)
                args.Add(new ParsedCommandArgument(sbCurrentData.ToString()));

            // Ensure something has been found
            if (args.Count == 0)
                throw new Exception($"Unable to parse '{inputCommand}' into a command");

            // Return the Parsed Command
            var result = new ParsedCommand()
            {
                Command = args[0].Value.ToUpper(),
                IsArgumentParameterSpecific = false,
            };

            // Remove the Command
            args.RemoveAt(0);

            if (args.Count > 0)
            {
                // Check if second argument is a Question Mark. If so, display help.
                if ((args[0].Parameter == "?") || (args[0].Value == "?"))
                    result.IsHelpRequest = true;
                else
                    result.Arguments = args.ToArray();
            }

            return result;
        }
    }
}
