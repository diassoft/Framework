using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace Diassoft.Mvvm.Command
{

    /// <summary>
    /// Represents a Relay Command to be used in MVVM.
    /// A Relay Command allows you to point to methods and functions to perform the commmand tasks.
    /// This command is compatible with Xamarin Forms.
    /// </summary>
    public partial class RelayCommand : CommandBase
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Initializes a new Relay Command
        /// </summary>
        /// <param name="execute">Delegate to the Command Action</param>
        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        /// <summary>
        /// Initializes a new Relay Command
        /// </summary>
        /// <param name="execute">Delegate to the Command Action</param>
        /// <param name="canExecute">Delegate to a Function that will validate if the command can be executed</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines if the command can be executed or not
        /// </summary>
        /// <param name="parameter">The command input parameter</param>
        /// <returns><see cref="bool"/>True when the Command Can be Executed, False when it cannot be executed</returns>
        public override bool CanExecute(object parameter)
        {
            return (_canExecute == null) ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">A generic input parameter of the command</param>
        public override void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }

}
