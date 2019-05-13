using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomeDetail {
    public partial class IncomeDetailPage : ContentPage {
        public IncomeDetailPage() {
            InitializeComponent();
            Title = "Your income";
            NavigationPage.SetHasNavigationBar(this, true);
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", async () => {
                await Navigation.PopModalAsync();
            }));

            amountEntry.Keyboard = Keyboard.Numeric;
            incomeDatePicker.Date = DateTime.Now;

            using (var scope = App.Container.BeginLifetimeScope()) { 
                if (scope.Resolve<IncomeDetailPageViewModel>() is IncomeDetailPageViewModel viewModel) {
                    BindingContext = viewModel;
                }
            }

        }

        protected async override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is IncomeDetailPageViewModel viewModel) {
                await viewModel.Update();
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

            if (BindingContext is IncomeDetailPageViewModel viewModel) {
                await viewModel.Save();
            }
            await Navigation.PopModalAsync();
        }
    }
}
