using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.Mvvm.Messenger;
using Diassoft.Mvvm.Command;
using System.Runtime.CompilerServices;

namespace Diassoft.Mvvm
{
    /// <summary>
    /// Defines the base class for a ViewModel
    /// </summary>
    public abstract partial class ViewModelBase: ObservableObjectBase
    {
        #region Command Registration

        /// <summary>
        /// Method to be overwritten to wire the internal commands
        /// </summary>
        public abstract void WireCommands();

        private Dictionary<string, CommandBase> RegisteredCommands;

        /// <summary>
        /// Register the Command 
        /// </summary>
        /// <param name="commandName">The Command Name</param>
        /// <param name="command">The Command Object</param>
        protected void RegisterCommand(string commandName, CommandBase command)
        {
            if (RegisteredCommands.ContainsKey(commandName)) return;
            RegisteredCommands.Add(commandName, command);
        }

        #endregion Command Registration

        #region Messaging

        private MessageDispatcher _dispatcher;

        /// <summary>
        /// The <see cref="MessageDispatcher">Message Dispatcher</see> attached to the operation.
        /// </summary>
        public MessageDispatcher Dispatcher
        {
            get { return _dispatcher; }
            set
            {
                SetProperty<MessageDispatcher>(ref _dispatcher, value, nameof(Dispatcher));

                // If dispatcher changed, subscribe to all messages
                if (_dispatcher != null) SubscribeToMessages();
            }
        }

        /// <summary>
        /// Override this method to setup the Message Subscriptions
        /// </summary>
        public abstract void SubscribeToMessages();

        #endregion Messaging

        /// <summary>
        /// Defines whether the ViewModel is initialized or not
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// Initializes the ViewModel
        /// </summary>
        public virtual void Initialize()
        {
            if (!IsInitialized) IsInitialized = true;
        }

        /// <summary>
        /// Defines whether the ViewModel is loaded or not
        /// </summary>
        public bool IsLoaded { get; protected set; }

        private bool _isBusy;
        /// <summary>
        /// Defines whether the ViewModel is busy at the momment
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty<bool>(ref _isBusy, value, nameof(IsBusy)); }
        }

        /// <summary>
        /// Method to be called after the ViewModel is loaded.
        /// It should be assigned to the Vew OnLoaded event handler.
        /// </summary>
        public virtual void OnLoaded()
        {
            if (IsLoaded) return;

            IsLoaded = true;
        }

        /// <summary>
        /// Method to be called when the view model is being uloaded.
        /// It should be assigned to the View OnUnloaded event handler.
        /// </summary>
        public virtual void OnUnloaded()
        {
            if (!IsLoaded) return;

            IsLoaded = false;
        }

        /// <summary>
        /// Initializes a new instance of the view model
        /// </summary>
        public ViewModelBase(): this(null) { }

        /// <summary>
        /// Initializes a new instance of the view model and tie it to the Dispatcher
        /// </summary>
        /// <param name="dispatcher">Reference to the <see cref="MessageDispatcher">MessageDispatcher</see></param>
        public ViewModelBase(MessageDispatcher dispatcher): base()
        {
            RegisteredCommands = new Dictionary<string, CommandBase>();
            WireCommands();
            Dispatcher = dispatcher;
        }

    }
}
