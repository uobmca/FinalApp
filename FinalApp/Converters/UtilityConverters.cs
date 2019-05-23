using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalApp.Converters {
    public class InverseBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool boolValue) {
                return !boolValue;
            }
            throw new ArgumentException("Trying to use InverseBoolConverter on something that isn't a boolean");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool boolValue) {
                return !boolValue;
            }
            throw new ArgumentException("Trying to use InverseBoolConverter on something that isn't a boolean");
        }
    }
}
