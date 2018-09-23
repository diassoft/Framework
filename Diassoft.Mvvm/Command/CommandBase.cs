using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Diassoft.Mvvm.Command
{
    /// <summary>
    /// The base class for a Mvvm Command
    /// </summary>
    public abstract partial class CommandBase : ObservableObjectBase, ICommand
    {
        /// <summary>
        /// Funcition to define whether the commmand can be executed or not
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Method to execute the commamnd action
        /// </summary>
        /// <param name="parameter">Command Input Parameter</param>
        public abstract void Execute(object parameter);

    }
}
