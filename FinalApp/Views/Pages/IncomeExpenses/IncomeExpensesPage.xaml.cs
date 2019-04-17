using System;
using System.Collections.Generic;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomeExpenses {
    public partial class IncomeExpensesPage : ContentPage {
        public IncomeExpensesPage() {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<IncomeExpensesPageViewModel>() is IncomeExpensesPageViewModel viewModel) {
                    BindingContext = viewModel;
                }
            }
        }
    }
}
