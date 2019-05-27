using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FinalApp.Views.Pages.SelectDateRange {

    public partial class SelectDateRangePage : ContentPage {

        public class FilterDateRange {
            public DateTime StartDate;
            public DateTime EndDate;
        }

        public Command<FilterDateRange> OnConfirmCommand { get; set; }

        public SelectDateRangePage() {
            InitializeComponent();
            startDatePicker.Date = DateTime.Now;
            endDatePicker.Date = DateTime.Now;
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", () => {
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            }));
        }

        async void Handle_Clicked(object sender, System.EventArgs e) {
            await Navigation.PopModalAsync();
            if (OnConfirmCommand != null) {
                OnConfirmCommand.Execute(new FilterDateRange {
                    StartDate = startDatePicker.Date,
                    EndDate = endDatePicker.Date
                });
            }
        }
    }
}
