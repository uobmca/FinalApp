using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FinalApp.Views.Pages.Dashboard {
    public partial class DashboardPage : ContentPage {
        public DashboardPage() {
            InitializeComponent();
        }

        void OnAddIncomeClicked(object sender, System.EventArgs e) {
            Navigation.PushModalAsync(new NavigationPage(new AddIncomePage.AddIncomePage()), true);
        }

        void OnAddExpensesClicked(object sender, System.EventArgs e) {
            Navigation.PushModalAsync(new NavigationPage(new AddIncomePage.AddIncomePage()), true);
        }
    }
}
