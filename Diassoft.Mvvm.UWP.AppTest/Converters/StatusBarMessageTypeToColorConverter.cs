using Diassoft.Mvvm.UWP.AppTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Diassoft.Mvvm.UWP.AppTest.Converters
{
    /// <summary>
    /// Converts a Status to a Color
    /// </summary>
    public class StatusBarMessageTypeToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
