using Diassoft.Mvvm.Wpf.AppTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Diassoft.Mvvm.Wpf.AppTest.Converters
{
    /// <summary>
    /// Converts a Status to a Color
    /// </summary>
    public class StatusBarMessageTypeToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Sets Value
            ViewModels.MessageTypes msgType = (ViewModels.MessageTypes)value;

            if (parameter.ToString().ToUpper() == "FONT")
            {
                if (msgType == ViewModels.MessageTypes.Success)
                    return "White";

                else if (msgType == ViewModels.MessageTypes.Error)
                    return "Yellow";

                else if (msgType == ViewModels.MessageTypes.Warning)
                    return "Black";

                else if (msgType == ViewModels.MessageTypes.Normal)
                    return "White";

                else
                    return "Black";
            }
            else
            {
                if (msgType == ViewModels.MessageTypes.Success)
                    return "Green";

                else if (msgType == ViewModels.MessageTypes.Error)
                    return "Red";

                else if (msgType == ViewModels.MessageTypes.Warning)
                    return "Yellow";

                else if (msgType == ViewModels.MessageTypes.Normal)
                    return "#007acc";

                else
                    return "Transparent";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
