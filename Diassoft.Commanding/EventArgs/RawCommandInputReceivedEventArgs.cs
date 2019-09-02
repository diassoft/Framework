using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents the Arguments for the Command Input Received event
    /// </summary>
    public class RawCommandInputReceivedEventArgs: System.EventArgs
    {
        /// <summary>
        /// The raw command without any parsing
        /// </summary>
        public string RawCommand { get; }

        /// <summary>
        /// Represents the string response for the raw command
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawCommandInputReceivedEventArgs"/>
        /// </summary>
        /// <param name="rawCommand">The raw command without any parsing</param>
        public RawCommandInputReceivedEventArgs(string rawCommand)
        {
            RawCommand = rawCommand;
        }

    }
}
