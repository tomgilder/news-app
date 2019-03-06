using System;
using System.Globalization;
using Xamarin.Forms;
using Humanizer;

namespace NewsApp
{
    public class HumanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTimeOffset))
            {
                throw new InvalidCastException("HumanizerConverter: value must be DateTimeOffset");
            }

            return ((DateTimeOffset)value).Humanize();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}