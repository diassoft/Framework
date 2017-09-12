using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Diassoft.Intelligence.Translators
{
    /// <summary>
    /// This class receives an string input and translates it to a valid date
    /// </summary>
    public class StringToDateTranslator : ITranslatable<string, DateTime>
    {
        /// <summary>
        /// Translates a String to a DateTime
        /// </summary>
        /// <param name="input">The string</param>
        /// <returns>A <see cref="DateTime"/> with the translated information</returns>
        public DateTime Translate(string input)
        {
            // Temporary reults
            StringBuilder sbStringContent = new StringBuilder();
            StringBuilder sbNumericContent = new StringBuilder();

            // Control Lists
            Dictionary<string, int> MonthsAbbreviatedDictionary = new Dictionary<string, int>();
            Dictionary<string, int> MonthsFullDictionary = new Dictionary<string, int>();

            // Remove any Special Characters and Blanks
            foreach (char c in input)
            {
                // Check character 
                if ((c >= 48) && (c <= 57))
                {
                    // 48 to 57 = from number 0 to 9
                    sbNumericContent.Append(c);
                }
                else if ((c >= 65) && (c <= 90))
                {
                    // 65 to 90 = from letter A to Z
                    sbStringContent.Append(c);
                }
                else if ((c >= 97) && (c <= 122))
                {
                    // 97 to 122 = from letter a to z 
                    // Capitalize string (32 is the difference between the lower case char code and upper case char code)
                    sbStringContent.Append((char)(c - 32));
                }
            }

            // Validate String Contents
            if (sbStringContent.Length > 0)
            {
                // Check "st" "nd" "rd" "th"
                if (sbStringContent.Length >= 2)
                {
                    string tmpEnding = sbStringContent.ToString(sbStringContent.Length - 2, 2);
                    if ((tmpEnding == "ST") ||
                         (tmpEnding == "ND") ||
                         (tmpEnding == "RD") ||
                         (tmpEnding == "TH"))
                    {
                        sbStringContent.Remove(sbStringContent.Length - 2, 2);
                    }
                }

                // Create Dictionary with the Proper Months
                for (int i = 0; i < CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames.Length-1; i++)
                {
                    MonthsAbbreviatedDictionary.Add(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[i].ToUpper(), i + 1);
                }

                // Check if current value is indeed a month
                for (int i = 0; i < CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Length - 1; i++)
                {
                    MonthsFullDictionary.Add(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i].ToUpper(), i + 1);
                }

            }




            return new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// Tries to translate a String to a DateTime
        /// </summary>
        /// <param name="input">The string</param>
        /// <param name="translation">A reference to a <see cref="DateTime"/> where the translation will be stored</param>
        /// <returns>A <see cref="bool"/> value to inform whether the translation was successfull or not</returns>
        public bool TryTranslate(string input, out DateTime translation)
        {
            throw new NotImplementedException();
        }

    }
}
