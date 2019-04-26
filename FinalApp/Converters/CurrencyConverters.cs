using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalApp.Converters {
    public class DoubleToCurrencyStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double currencyValue) {
                return string.Format("{0:C}", currencyValue);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
