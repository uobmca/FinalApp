using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.ExpenseDetail {
    public partial class ExpenseDetailPage : ContentPage {
        public ExpenseDetailPage() {
            InitializeComponent();
            Title = "Your expense";
            NavigationPage.SetHasNavigationBar(this, true);
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", async () => {
                await Navigation.PopModalAsync();
            }));

            amountEntry.Keyboard = Keyboard.Numeric;
            expireDatePicker.Date = DateTime.Now;

            using (var scope = App.Container.BeginLifetimeScope()) { 
                if(scope.Resolve<ExpenseDetailPageViewModel>() is ExpenseDetailPageViewModel viewModel) {
                    BindingContext = viewModel;
                    viewModel.Update();
                }
            }


         }

        protected async override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is ExpenseDetailPageViewModel viewModel) {
                if (viewModel.SelectedUserCategory == null) {
                    viewModel.SelectedUserCategory = viewModel.UserCategories.FirstOrDefault();
                    categoryPicker.SelectedItem = viewModel.SelectedUserCategory;
                }
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e) {

            if (amountEntry.Text.Trim().Equals("") || categoryPicker.SelectedItem == null) {
                await DisplayAlert("Error", "Please, complete required fields to proceed.", "Ok");
                return;
            }

            var regex = new Regex("^\\d+([\\.,]*)\\d+$");
            if (!regex.IsMatch(amountEntry.Text.Trim())) {
                await DisplayAlert("Error", "The inserted amount is not valid.", "Ok");
                return;
            }

            if (BindingContext is ExpenseDetailPageViewModel viewModel) {
                await viewModel.Save();
                //await Navigation.PopModalAsync();
                await Navigation.PopModalAsync();
            }
        }
    }
}
