using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalApp.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DateRange : ContentView
	{
        public static readonly BindableProperty RangeTextProperty = BindableProperty.Create(
                                                         propertyName: "RangeText",
                                                         returnType: typeof(string),
                                                         declaringType: typeof(DateRange),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: RangeTextPropertyChanged);

        public string RangeText
        {   
            get { return base.GetValue(RangeTextProperty).ToString(); }
            set { base.SetValue(RangeTextProperty, value); }
        }

        private static void RangeTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DateRange)bindable;
            control.range.Text = newValue.ToString();
        }
        
        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(
            nameof(MinimumDate), 
            typeof(DateTime), 
            typeof(DatePicker), 
            new DateTime(1900, 1, 1),
            validateValue: ValidateMinimumDate, 
            coerceValue: CoerceMinimumDate);
        
        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        static object CoerceMinimumDate(BindableObject bindable, object value)
        {
            DateTime dateValue = ((DateTime)value).Date;
            var picker = (DatePicker)bindable;
            if (picker.Date < dateValue)
                picker.Date = dateValue;

            return dateValue;
        }
        
        static bool ValidateMinimumDate(BindableObject bindable, object value)
        {
            return (DateTime)value <= ((DatePicker)bindable).MaximumDate;
        }

        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(
            nameof(MaximumDate), 
            typeof(DateTime), 
            typeof(DatePicker), 
            new DateTime(2100, 12, 31),
            validateValue: ValidateMaximumDate, 
            coerceValue: CoerceMaximumDate);

        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        static bool ValidateMaximumDate(BindableObject bindable, object value)
        {
            return (DateTime)value >= ((DatePicker)bindable).MinimumDate;
        }


        static object CoerceMaximumDate(BindableObject bindable, object value)
        {
            DateTime dateValue = ((DateTime)value).Date;
            var picker = (DatePicker)bindable;
            if (picker.Date > dateValue)
                picker.Date = dateValue;

            return dateValue;
        }
        
        public DateRange ()
		{
			InitializeComponent ();
		}
	}
}