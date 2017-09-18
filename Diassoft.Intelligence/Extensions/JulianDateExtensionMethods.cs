using Diassoft.Intelligence.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Intelligence.Extensions
{
    /// <summary>
    /// Extension Methods to Implement Julian Date Conversion
    /// </summary>
    public static class JulianDateExtensionMethods
    {

        #region From DateTime to Julian

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a Julian Date
        /// </summary>
        /// <param name="gregorian">The input DateTime</param>
        /// <returns>An <see cref="int"/> containing the Julian Date.</returns>
        public static int ToJulian(this DateTime gregorian)
        {
            // Minimum Value means zero
            if (gregorian == DateTime.MinValue) return 0;
            if (gregorian.Year < 1900) throw new InvalidJulianDateException(gregorian);

            return ((gregorian.Year - 1900) * 1000) + (gregorian.DayOfYear);
        }

        /// <summary>
        /// Try to convert a Gregorian Date to a Julian Date
        /// </summary>
        /// <param name="gregorian">The Gregorian Date (a <see cref="System.DateTime">DateTime</see> field with a date greater or equal to Jan 1st 1900)</param>
        /// <param name="julian">Reference to the <see cref="System.Int16">int</see> variable where the results will be stored</param>
        /// <returns>True if the Gregorian Date is valid, False if not.</returns>
        public static bool TryConvertToJulian(this DateTime gregorian, out int julian)
        {
            try
            {
                julian = ToJulian(gregorian);
                return true;
            }
            catch
            {
                julian = -1;
                return false;
            }
        }

        #endregion From DateTime to Julian

        #region From Julian to DateTime

        /// <summary>
        /// Converts a Julian Date to Gregorian Date
        /// </summary>
        /// <param name="dt">Reference to the DateTime calling object</param>
        /// <param name="julian">Julian Date (a number between 1 and 999366)</param>
        /// <returns></returns>
        public static DateTime CreateFromJulian(this DateTime dt, int julian)
        {
            // Zero returns the Minimum Value
            if (julian == 0) return DateTime.MinValue; 

            // Check number of characters (must be between 1 and 999366)
            if ((julian < 1) || (julian > 999999)) throw new InvalidJulianDateException(julian);

            // Retrieve the number of days on the year
            int days = julian % 1000;

            // Retrieve the year
            int year = 1900 + ((julian - days) / 1000);

            // Check for Leap Year
            if ((days > 365) && (year % 4 > 0)) throw new InvalidJulianDateException(julian);
            if ((days > 366) && (year % 4 == 0)) throw new InvalidJulianDateException(julian);

            // Return the Gregorian Date   
            return new DateTime(year, 1, 1).AddDays(days - 1);
        }

        #endregion From Julian to DateTime

    }
}
