using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalApp.Converters {
    public class DateTimeOffsetToDateTimeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DateTimeOffset dateTime) {
                return dateTime.DateTime;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DateTime dateTime) {
                return new DateTimeOffset(dateTime);
            }
            return value;
        }
    }
}
