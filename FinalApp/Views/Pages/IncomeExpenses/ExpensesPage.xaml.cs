using System;
using System.Collections.Generic;
using Autofac;
using FinalApp.Commons;
using FinalApp.ViewModels;
using FinalApp.Views.Base;
using FinalApp.Views.Pages.ExpensesList;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomeExpenses {
    public partial class ExpensesPage : ContentPage {

        private ExpensesPageViewModel viewModel;

        public ExpensesPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            ToolbarItems.Add(new ToolbarItem("Add", "ic_add", async () => {
                await Navigation.PushModalAsyncUnique(new NavigationPage(new AddIncomePage.AddIncomePage()));
            }));

            if (DesignMode.IsDesignModeEnabled) return;
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<ExpensesPageViewModel>() is ExpensesPageViewModel viewModel) {
                    BindingContext = viewModel;
                    this.viewModel = viewModel;
                }
            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {
            if (e.SelectedItem is ExpensesGroupedList expenseList) {
                using (var scope = App.Container.BeginLifetimeScope()) {
                    if (scope.Resolve<ExpensesListPageViewModel>() is ExpensesListPageViewModel viewModel) {
                        var page = new ExpensesListPage();
                        viewModel.CategoryId = expenseList.ExpensesCategoryId;
                        page.BindingContext = viewModel;
                        Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
                    }
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
