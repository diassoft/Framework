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

        #region Static Members

        /// <summary>
        /// Combinations to be generated for each value member of a date.
        /// Those with a "-" is to be used when having separators added on the string to be parsed
        /// </summary>
        internal static Dictionary<string, string[]> Combinations { get; } = new Dictionary<string, string[]>()
        {
            { "D", new string[] { "dd", "d" } },
            { "M", new string[] { "MMMM", "MMM", "MM", "M" } },
            { "Y", new string[] { "yyyy", "yy", "y" } },
            { "D-", new string[] { "dd", "d" } },
            { "M-", new string[] { "MM", "M" } },
            { "Y-", new string[] { "yyyy", "yy", "y" } }
        };

        /// <summary>
        /// Represents the Formats Available per Culture
        /// </summary>
        public static Dictionary<string, CultureAdditionalInformation> Formats { get; } = new Dictionary<string, CultureAdditionalInformation>();

        /// <summary>
        /// Method to create the DateFormats for the given culture
        /// </summary>
        /// <param name="culture">A string representing the culture</param>
        public static void CreateDateFormatsForCulture(string culture)
        {
            // Check if Culture is already populated
            if (StringToDateTranslator.Formats.ContainsKey(culture)) return;

            // Retrieve Culture Formatting
            CultureInfo newCulture = new CultureInfo(culture);

            // Look for separator
            CultureAdditionalInformation CultureAdditionalInfo = new Translators.CultureAdditionalInformation()
            {
                DateSeparator = '\0',
                DateFormatsWithoutSeparator = new string[] { },
                DateFormatsWithSeparator = new string[] { },
                DaysOfTheWeek = new HashSet<string>(),
                DaysOfTheWeekAbbreviated = new HashSet<string>()
            };

            // Add the Days of the Week
            foreach (string s in newCulture.DateTimeFormat.DayNames)
            {
                // As it is version
                string tmpString = s.ToUpper();
                CultureAdditionalInfo.DaysOfTheWeek.Add(tmpString);

                // Abbreviated Version
                tmpString = tmpString.Substring(0, 3);
                if (!CultureAdditionalInfo.DaysOfTheWeekAbbreviated.Contains(tmpString))
                    CultureAdditionalInfo.DaysOfTheWeekAbbreviated.Add(tmpString);
            }

            // Array of Properties of the Date Format
            string[] dateFormatArray;

            foreach (char c in newCulture.DateTimeFormat.ShortDatePattern)
            {
                if ( ((c >= 33) && (c <= 47)) ||
                     ((c >= 58) && (c <= 64)) ||
                     ((c >= 91) && (c <= 96)) ||
                     ((c >= 123) && (c <= 126)) )
                {
                    CultureAdditionalInfo.DateSeparator = c;
                    break;
                }
            }

            // Make sure a separator was found
            if (CultureAdditionalInfo.DateSeparator != '\0')
            {
                dateFormatArray = newCulture.DateTimeFormat.ShortDatePattern.Split(CultureAdditionalInfo.DateSeparator);

                // Keep the only string character on the array
                for (int i = 0; i < dateFormatArray.Length; i++)
                {
                    dateFormatArray[i] = dateFormatArray[i].Replace(" ","")[0].ToString().ToUpper();
                }

                // Check if dateFormatArray is valid
                if (dateFormatArray.Length < 3) return;

                // Now prepare all the possible combinations, following the dateFormatArray format
                List<string> tempCombinations = new List<string>();

                // Check Combinations without the separator
                foreach (string c1 in Combinations[dateFormatArray[0]])
                {
                    foreach (string c2 in Combinations[dateFormatArray[1]])
                    {
                        foreach (string c3 in Combinations[dateFormatArray[2]])
                        {
                            tempCombinations.Add($"{c1}{c2}{c3}");
                        }
                    }
                }

                CultureAdditionalInfo.DateFormatsWithoutSeparator = tempCombinations.ToArray();

                tempCombinations.Clear();

                // Check Combinations with the the separator
                foreach (string c1 in Combinations[$"{dateFormatArray[0]}-"])
                {
                    foreach (string c2 in Combinations[$"{dateFormatArray[1]}-"])
                    {
                        foreach (string c3 in Combinations[$"{dateFormatArray[2]}-"])
                        {
                            tempCombinations.Add($"{c1}{CultureAdditionalInfo.DateSeparator}{c2}{CultureAdditionalInfo.DateSeparator}{c3}");
                        }
                    }
                }

                CultureAdditionalInfo.DateFormatsWithSeparator = tempCombinations.ToArray();

                // Add to Formats
                Formats.Add(culture, CultureAdditionalInfo);
            }
        }

        #endregion Static Members

        #region Properties

        /// <summary>
        /// The Culture in Use to Perform the Translation
        /// </summary>
        public CultureInfo Culture { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the String To Date Translator
        /// </summary>
        /// <param name="culture">The code representing the culture</param>
        public StringToDateTranslator(string culture)
        {
            // Initialize the Culture Formats if not initialized yet
            StringToDateTranslator.CreateDateFormatsForCulture(culture);
            Culture = new CultureInfo(culture);
        }

        /// <summary>
        /// ///  Initializes a new instance of the String To Date Translator
        /// </summary>
        public StringToDateTranslator() : this(CultureInfo.CurrentCulture.Name)
        {
            // Add the default culture
            StringToDateTranslator.CreateDateFormatsForCulture("en-US");
        }

        #endregion Constructors

        #region ITranslatable Implementation

        /// <summary>
        /// Translates a String to a DateTime
        /// </summary>
        /// <param name="input">The string to be translated</param>
        /// <returns>A <see cref="DateTime"/> with the translated information</returns>
        public DateTime Translate(string input)
        {
            // Hard coded values
            if ((input == "T") || (input == "t")) return DateTime.Now.Date;

            // Internal Variables
            DateTime date = DateTime.MinValue;

            // Single Digit / Double Digit Numbers (to represent the day of the month)
            if (byte.TryParse(input, out byte inputNumber))
            {
                // This is a valid number, try to return a date with that
                if ((inputNumber >= 1) && (inputNumber <= 31))
                {
                    // This is a good candidate, try to parse a date with that
                    if (DateTime.TryParseExact($"{DateTime.Now.ToString("yyyyMM")}{inputNumber.ToString().PadLeft(2, '0')}",
                                               "yyyyMMdd",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out date)) return date;
                }
            }

            // Check for Parsing Requirements
            if (Culture == null) throw new Exception($"There is no {nameof(Culture)} defined for the translation. Unable to parse dates.");
            if (!StringToDateTranslator.Formats.ContainsKey(Culture.Name)) throw new Exception($"The system could not create the culture format. Unable to parse dates.");

            // Internal Variables
            CultureAdditionalInformation cultureAdditionalInfo = StringToDateTranslator.Formats[Culture.Name];

            //TODO: Check Days of the Week
            

            // Try all formats with the date sparator
#if DEBUG
            foreach (string format in cultureAdditionalInfo.DateFormatsWithSeparator)
            {
                if (DateTime.TryParseExact(input,
                                           format,
                                           Culture,
                                           DateTimeStyles.None,
                                           out date))
                    return date;
            }
#else
            if (DateTime.TryParseExact(input,
                                       cultureAdditionalInfo.WithSeparator,
                                       Culture,
                                       DateTimeStyles.None,
                                       out date))
                return date;
#endif

            // Temporary reults
            StringBuilder sbInput = new StringBuilder();

            // Remove any Special Characters and Blanks
            foreach (char c in input)
            {
                // Check character 
                if ((c >= 48) && (c <= 57))
                {
                    // 48 to 57 = from number 0 to 9
                    sbInput.Append(c);
                }
                else if ((c >= 65) && (c <= 90))
                {
                    // 65 to 90 = from letter A to Z
                    sbInput.Append(c);
                }
                else if ((c >= 97) && (c <= 122))
                {
                    // 97 to 122 = from letter a to z 
                    // Capitalize string (32 is the difference between the lower case char code and upper case char code)
                    sbInput.Append(c);
                }
            }

            // Try all formats without the date sparator
#if DEBUG
            foreach (string format in cultureAdditionalInfo.DateFormatsWithoutSeparator)
            {
                if (DateTime.TryParseExact(sbInput.ToString(),
                                           format,
                                           Culture,
                                           DateTimeStyles.None,
                                           out date))
                    return date;
            }

            // No format was accepted, give up
            throw new Exception($"The value of '{input}' could not be interpreted as a valid date");
#else
            if (DateTime.TryParseExact(sbInput.ToString(),
                                       cultureAdditionalInfo.WithoutSeparator,
                                       Culture,
                                       DateTimeStyles.None,
                                       out date))
                return date;
            else
                throw new Exception($"The value of '{input}' could not be interpreted as a valid date");
#endif

            // Nothing worked, try to inject the separator

        }

        /// <summary>
        /// Tries to translate a String to a DateTime
        /// </summary>
        /// <param name="input">The string</param>
        /// <param name="translation">A reference to a <see cref="DateTime"/> where the translation will be stored</param>
        /// <returns>A <see cref="bool"/> value to inform whether the translation was successfull or not</returns>
        public bool TryTranslate(string input, out DateTime translation)
        {            
            try
            {
                translation = Translate(input);
                return true;
            }
            catch 
            {
                translation = DateTime.MinValue;
                return false;
            }
        }

        #endregion ITranslatable Implementation

    }

    /// <summary>
    /// Structure with the Format Details
    /// </summary>
    public struct CultureAdditionalInformation
    {
        /// <summary>
        /// Array of formats with separator
        /// </summary>
        public string[] DateFormatsWithSeparator;

        /// <summary>
        /// Array of formats without the separator
        /// </summary>
        public string[] DateFormatsWithoutSeparator;

        /// <summary>
        /// Date Separator
        /// </summary>
        public char DateSeparator;

        /// <summary>
        /// Represents a <see cref="HashSet{T}"/> containing the Days of the Week
        /// </summary>
        /// <remarks>All data is capitalized</remarks>
        public HashSet<string> DaysOfTheWeek;

        /// <summary>
        /// Represents a <see cref="HashSet{T}"/> containing the Days of the Week Abbreviated (just the first three digits)
        /// </summary>
        /// <remarks>All data is capitalized</remarks>
        public HashSet<string> DaysOfTheWeekAbbreviated;
    }
    
}
