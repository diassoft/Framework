using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Diassoft.Mvvm
{
    /// <summary>
    /// Represents a Mvvm Observable Object
    /// All classes that inherit from this class must implement the SetProperty of the Setters inside properties.
    /// </summary>
    public abstract class ObservableObjectBase: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        /// <summary>
        /// The event triggered when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event handler for the On Property Changed Event
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the Property Value if it changed. Notify proper listeners.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="storage">Reference to the Property (must have a get and set)</param>
        /// <param name="value">Desired value for the property</param>
        /// <param name="propertyName">The property name itself</param>
        /// <returns>
        /// <see cref="bool">True / False</see> to define whether the value changed or not.
        /// </returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }

        #endregion INotifyPropertyChanged Implementation


    }
}
