using System;
using System.Collections.Generic;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class MonthlyReportRowTemplate : ViewCell {
        public MonthlyReportRowTemplate() {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e) {
            if (BindingContext is MonthlyReport report) {
                report.OpenButtonClicked();
            }
        }
    }
}
