using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Event Arguments for a Session Ended Event
    /// </summary>
    public sealed class SessionEndedEventArgs : System.EventArgs
    {
        /// <summary>
        /// ID of the Session
        /// </summary>
        public string SessionID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionEndedEventArgs"/>
        /// </summary>
        /// <param name="sessionID">ID of the Session</param>
        public SessionEndedEventArgs(string sessionID)
        {
            SessionID = sessionID;
        }

    }
}
