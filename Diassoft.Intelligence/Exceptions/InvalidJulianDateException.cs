using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Intelligence.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the <see cref="JulianDate">Julian Date</see> is invalid.
    /// </summary>
    public class InvalidJulianDateException : System.Exception
    {
        /// <summary>
        /// The Julian Date
        /// </summary>
        public int JulianDate { get; set; }

        /// <summary>
        /// The Gregorian Date
        /// </summary>
        public DateTime GregorianDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidJulianDateException"/>.
        /// </summary>
        /// <param name="julianDate">The Julian Date</param>
        public InvalidJulianDateException(int julianDate) : base($"{julianDate} is an invalid Julian Date. It should be a value between 1 and 999365")
        {
            JulianDate = julianDate;
            GregorianDate = DateTime.MinValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidJulianDateException"/>.
        /// </summary>
        /// <param name="gregorianDate">The gregorian date</param>
        public InvalidJulianDateException(DateTime gregorianDate) : base($"{gregorianDate.ToString("yyyy-MM-dd")} is an invalid Gregorian Date. Year should not be less than 1900.")
        {
            JulianDate = 0;
            GregorianDate = gregorianDate;
        }

    }
}
