using Diassoft.Mvvm.Command;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

// This is only implemented for .NET STD and .NET CORE

#if (NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1)

namespace Diassoft.Mvvm
{
    public abstract partial class ViewModelBase : ObservableObjectBase
    {
        #region Command Registration

        /// <summary>
        /// Method to Requery The Command Status.
        /// </summary>
        protected void RequeryCommands()
        {
            // Check for commands
            if (RegisteredCommands?.Count == 0) return;

            // Call Property Changed on each command
            foreach (KeyValuePair<string, CommandBase> kvp in RegisteredCommands)
                kvp.Value.Requery();
        }

        #endregion Command Registration

        /// <summary>
        /// Sets the Property Value if it changed. Notify proper listeners.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="storage">Reference to the Property (must have a get and set)</param>
        /// <param name="value">Desired value for the property</param>
        /// <param name="propertyName">The property name itself</param>
        /// <returns>
        /// True if the value has changed. False if the value hasn't changed.
        /// </returns>
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            // Call the ObservableObject SetProperty
            if (!base.SetProperty<T>(ref storage, value, propertyName)) return false;

            // Requery Commands Wired on ViewModel when not running thru .NET Framework (.NET framework handles that with the CommandManager
            RequeryCommands();

            return true;
        }
    }
}

#endif