using System;
using System.Collections.Generic;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomeExpenses {
    public partial class ExpensesPage : ContentPage {

        private ExpensesPageViewModel viewModel;

        public ExpensesPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (DesignMode.IsDesignModeEnabled) return;
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<ExpensesPageViewModel>() is ExpensesPageViewModel viewModel) {
                    BindingContext = viewModel;
                    this.viewModel = viewModel;
                }
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            if (viewModel == null) return;
            viewModel.Update();
        }
    }
}
