using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace Diassoft.Mvvm.Command
{
    /// <summary>
    /// Represents a Relay Command. The Command Design Pattern is used in MVVM.
    /// A RelayCommand is a generic implementation of a command that accepts one parameter of any given type (using generics).
    /// </summary>
    /// <typeparam name="T">The <see cref="Type">Type</see> of the Parameter that the command will receive. If there is no strong-typed parameter, use <see cref="Object">object.</see></typeparam>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of a Relay Command
        /// </summary>
        /// <param name="execute">Reference to the method which will perform the command action</param>
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of a Relay Command
        /// </summary>
        /// <param name="execute">Reference to the method which will perform the command action</param>
        /// <param name="canExecute">Reference to the function to define whether the command can be executed or not</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        /// <summary>
        /// Checks if a command can be executed
        /// </summary>
        /// <param name="parameter">The command parameter</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

#if NET451 || NET46 || NET461

        /// <summary>
        /// Event triggered when the CanExecute property has changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

#else

        /// <summary>
        /// Event triggered when the CanExecute property has changed
        /// </summary>
        public event EventHandler CanExecuteChanged { add { } remove { } }

#endif

        /// <summary>
        /// Executes the Command Action
        /// </summary>
        /// <param name="parameter">The command parameter</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion // ICommand Members
    }


    /// <summary>
    /// Represents a Relay Command to be used in MVVM.
    /// A Relay Command allows you to point to methods and functions to perform the commmand tasks.
    /// This command is compatible with Xamarin Forms.
    /// </summary>
    public class RelayCommand : CommandBase, ICommand
    {
        private readonly Action<object> _handler;
        private readonly Predicate<object> _canExecute;
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new Relay Command
        /// </summary>
        /// <param name="handler">Delegate to the Command Action</param>
        public RelayCommand(Action<object> handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Initializes a new Relay Command
        /// </summary>
        /// <param name="handler">Delegate to the Command Action</param>
        /// <param name="canExecute">Delegate to a Function that will validate if the command can be executed</param>
        public RelayCommand(Action<object> handler, Predicate<object> canExecute)
        {
            _handler = handler;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines whether the command is enabled or not
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
#if NETSTANDARD1_2
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
#endif
                }
            }
        }

        /// <summary>
        /// Defines if the command can be executed or not
        /// </summary>
        /// <param name="parameter">The command input parameter</param>
        /// <returns><see cref="bool"/>True when the Command Can be Executed, False when it cannot be executed</returns>
        public override bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        /// <summary>
        /// Verify if the command can be executed. This method is used internally to validate the commands.
        /// </summary>
        public override void Requery()
        {
            if (_canExecute != null) IsEnabled = _canExecute(null);
        }

        /// <summary>
        /// Verify if the command can be executed. This method is used internally to validate the commands.
        /// </summary>
        /// <param name="parameter">The Command Input Parameter</param>
        public override void Requery(object parameter)
        {
            if (_canExecute != null) IsEnabled = _canExecute(parameter);
        }


#if NETSTANDARD1_2

        /// <summary>
        /// Event to be triggered when the CanExecute value changes
        /// </summary>
        public event EventHandler CanExecuteChanged;

#else
        /// <summary>
        /// Event triggered when the CanExecute property has changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

#endif

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">A generic input parameter of the command</param>
        public override void Execute(object parameter)
        {
            _handler(parameter);
        }
    }

}
