using System;
using System.Collections.Generic;
using System.Linq;
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
            if(BindingContext is IncomeDetailPageViewModel viewModel) {
                await viewModel.Save();
            }
            await Navigation.PopModalAsync();
        }
    }
}
