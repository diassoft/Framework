using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Mvvm.Messenger
{
    /// <summary>
    /// Represents the Base Class for a Message
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// The sender of the message
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// Initializes a new instance of the MessageBase class
        /// </summary>
        public MessageBase()
        {
            Sender = null;
        }

        /// <summary>
        /// Initializes a new instance of the MessageBase Class
        /// </summary>
        /// <param name="sender">The sender of the message</param>
        public MessageBase(object sender)
        {
            Sender = sender;
        }
    }
}
