using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Mvvm
{
    /// <summary>
    /// The base class for a Mvvm Command
    /// </summary>
    public abstract class CommandBase : ObservableObjectBase
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

        /// <summary>
        /// Method that recalculate the command availability to run
        /// </summary>
        public abstract void Requery();

        /// <summary>
        /// Method that recalculate the command availability to run
        /// </summary>
        /// <param name="parameter">Command Input Parameter</param>
        public abstract void Requery(object parameter);
    }
}
