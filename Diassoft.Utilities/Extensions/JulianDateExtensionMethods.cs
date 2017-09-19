using System;
using System.Collections.Generic;
using System.Text;
using Diassoft.Utilities;

namespace Diassoft.Utilities.Extensions
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
            return JulianDate.ConvertToJulian(gregorian);
        }

        /// <summary>
        /// Try to convert a Gregorian Date to a Julian Date
        /// </summary>
        /// <param name="gregorian">The Gregorian Date (a <see cref="System.DateTime">DateTime</see> field with a date greater or equal to Jan 1st 1900)</param>
        /// <param name="julian">Reference to the <see cref="System.Int16">int</see> variable where the results will be stored</param>
        /// <returns>True if the Gregorian Date is valid, False if not.</returns>
        public static bool TryConvertToJulian(this DateTime gregorian, out int julian)
        {
            return JulianDate.TryConvertToJulian(gregorian, out julian);
        }

        #endregion From DateTime to Julian

    }
}
