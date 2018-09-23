using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

// This is only implemented for .NET Framework

#if (NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

namespace Diassoft.Mvvm.Command
{
    public abstract partial class CommandBase : ObservableObjectBase, ICommand
    {
        /// <summary>
        /// Event triggered when the CanExecute value changes
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

    }

}

#endif