using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

// This is only implemented for .NET Framework

#if (NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472)

namespace Diassoft.Mvvm
{
    public abstract partial class ViewModelBase : ObservableObjectBase
    {
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

            return true;
        }
    }
}

#endif