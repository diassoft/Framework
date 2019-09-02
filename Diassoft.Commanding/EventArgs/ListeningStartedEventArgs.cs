using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Event Arguments for the Listening Started event
    /// </summary>
    public sealed class ListeningStartedEventArgs : System.EventArgs
    {
        /// <summary>
        /// The port where listening is happening
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListeningStartedEventArgs"/>
        /// </summary>
        /// <param name="port">Port where the system is listening</param>
        public ListeningStartedEventArgs(int port)
        {
            Port = port;
        }

    }
}
