using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Utilities
{
    /// <summary>
    /// Represents a class to provide Static Methods that convert julian dates
    /// </summary>
    public static class JulianDate
    {

        /// <summary>
        /// Converts a Julian Date to Gregorian Date
        /// </summary>
        /// <param name="julian">Julian Date (a number between 1 and 999366)</param>
        /// <returns></returns>
        public static DateTime ConvertToGregorian(int julian)
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

        /// <summary>
        /// Try to convert a Julian Date to a Gregorian Date
        /// </summary>
        /// <param name="julian">The Julian Date (a number between 1 and 999366)</param>
        /// <param name="gregorian">Reference to the <see cref="System.DateTime">DateTime</see> variable where the results will be stored</param>
        /// <returns>True if the Julian Date is valid. False if not.</returns>
        public static bool TryConvertToGregorian(int julian, out DateTime gregorian)
        {
            try
            {
                gregorian = ConvertToGregorian(julian);
                return true;
            }
            catch
            {
                gregorian = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Converts a Gregorian Date to a Julian Date
        /// </summary>
        /// <param name="gregorian">The Gregorian Date (a <see cref="System.DateTime">DateTime</see> field with a date greater or equal to Jan 1st 1900)</param>
        /// <returns>The Julian Date</returns>
        public static int ConvertToJulian(DateTime gregorian)
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
        public static bool TryConvertToJulian(DateTime gregorian, out int julian)
        {
            try
            {
                julian = ConvertToJulian(gregorian);
                return true;
            }
            catch
            {
                julian = -1;
                return false;
            }
        }

    }
}
