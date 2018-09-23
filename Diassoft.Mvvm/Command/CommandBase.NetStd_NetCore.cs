using System;   
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

// This is only implemented for .NET STD and .NET CORE

#if (NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1)

namespace Diassoft.Mvvm.Command
{
    public abstract partial class CommandBase : ObservableObjectBase, ICommand
    {
        private bool _isEnabled;

        /// <summary>
        /// Defines whether the command is enabled or not
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            protected set { SetProperty<bool>(ref _isEnabled, value, nameof(IsEnabled)); }
        }

        /// <summary>
        /// Event Triggered when the CanExecute value changes
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Method that recalculate the command availability to run
        /// </summary>
        public virtual void Requery()
        {
            Requery(null);
        }

        /// <summary>
        /// Method that recalculate the command availability to run
        /// </summary>
        /// <param name="parameter">Command Input Parameter</param>
        public virtual void Requery(object parameter)
        {
            // Check whether it's possible to execute the command or not
            bool _newStatus = CanExecute(parameter);

            // If status changed, call event
            if (_newStatus != IsEnabled)
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            IsEnabled = _newStatus;
        }
    }
}

#endif