using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Event Arguments for a Session Started Event
    /// </summary>
    public sealed class SessionStartedEventArgs: System.EventArgs
    {
        /// <summary>
        /// ID of the Session
        /// </summary>
        public string SessionID { get; }
        /// <summary>
        /// Defines whether the session should be denied
        /// </summary>
        public bool Deny { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStartedEventArgs"/>
        /// </summary>
        /// <param name="sessionID">ID of the Session</param>
        public SessionStartedEventArgs(string sessionID)
        {
            SessionID = sessionID;
        }

    }
}
