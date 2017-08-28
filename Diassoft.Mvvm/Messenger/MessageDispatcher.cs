using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Mvvm.Messenger
{
    /// <summary>
    /// Definition of the Message Receiver that will intercept messages.
    /// </summary>
    /// <param name="sender">The object sending the message</param>
    /// <param name="message">The message itself</param>
    public delegate void MessageReceiver(object sender, MessageBase message);

    /// <summary>
    /// Represents a Class that Exchange Messages for Mvvm Implementation
    /// </summary>
    public sealed class MessageDispatcher
    {

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}">Dictionary</see> containing the subscribers callbacks of a message.
        /// </summary>
        public Dictionary<Type, List<MessageReceiver>> Subscribers { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Message Dispatcher
        /// </summary>
        public MessageDispatcher()
        {
            // Initializes the Dictionary of Subscribers, by type
            Subscribers = new Dictionary<Type, List<MessageReceiver>>();
        }

        /// <summary>
        /// Sends a Message using the Dispatcher
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="sender">Reference to the object throwing the message</param>
        /// <param name="message">The Message to be sent</param>
        public void Send<T>(object sender, T message) where T : MessageBase
        {
            // Look for Subscribers
            List<MessageReceiver> subscribersCallBack = null;

            if (Subscribers.TryGetValue(typeof(T), out subscribersCallBack))
            {
                // Make sure the result is not null
                if (subscribersCallBack == null) return;

                // Subscribers were found, notify all them
                foreach (MessageReceiver subscriber in subscribersCallBack)
                {
                    subscriber(subscribersCallBack, message);
                }
            }
        }

        /// <summary>
        /// Subscribe to a Message
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="requester">Reference to the object subscribing to the message</param>
        /// <param name="callbackMethod">The delegate to a callback method that will be called once the message is thrown</param>
        /// <returns></returns>
        public bool Subscribe<T>(object requester, MessageReceiver callbackMethod) where T : MessageBase
        {
            // Look for the existing list of subscribers
            List<MessageReceiver> subscribersCallBack = null;

            if (Subscribers.TryGetValue(typeof(T), out subscribersCallBack))
            {
                // There is a valid list

                // Make sure it's not a null list, if so, initializes it
                if (subscribersCallBack == null)
                {
                    // Initializes it with the call back method delegate
                    subscribersCallBack = new List<MessageReceiver>() { callbackMethod };

                    // Updates Dictionary
                    Subscribers[typeof(T)] = subscribersCallBack;

                    return true;
                }
                else
                {
                    // Check if it doesn't exists already
                    if (subscribersCallBack.Contains(callbackMethod)) return false;

                    // Add the CallBackMethod
                    subscribersCallBack.Add(callbackMethod);

                    return true;
                }
            }
            else
            {
                // There is no list of subscribers, add it then
                subscribersCallBack = new List<MessageReceiver>() { callbackMethod };

                // Add the Subscription to the Collection
                Subscribers.Add(typeof(T), subscribersCallBack);

                return true;
            }

        }

    }
}
