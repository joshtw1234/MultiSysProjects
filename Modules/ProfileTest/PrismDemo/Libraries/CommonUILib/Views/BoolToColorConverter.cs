using System;
using System.Globalization;
using System.Windows.Data;

namespace CommonUILib.Views
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((bool)value)
            {
                value = "Red";
            }
            else
            {
                value = "Green";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
