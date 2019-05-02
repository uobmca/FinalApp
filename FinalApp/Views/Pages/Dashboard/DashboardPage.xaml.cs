using System;
using System.Collections.Generic;
using FinalApp.Commons;
using FinalApp.Views.Base;
using Xamarin.Forms;
using Autofac;
using FinalApp.ViewModels;

namespace FinalApp.Views.Pages.Dashboard {
    public partial class DashboardPage : ContentPage {
        public DashboardPage() {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<DashboardPageViewModel>() is DashboardPageViewModel viewModel) {
                    BindingContext = viewModel;
                    viewModel.Update();
                }
            }
        }

        async void OnAddIncomeClicked(object sender, System.EventArgs e) {
            var page = new IncomeDetail.IncomeDetailPage();
            await Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
        }

        void OnAddExpensesClicked(object sender, System.EventArgs e) {
            Navigation.PushModalAsyncUnique(new NavigationPage(new AddIncomePage.AddIncomePage()));
        }
    }
}
