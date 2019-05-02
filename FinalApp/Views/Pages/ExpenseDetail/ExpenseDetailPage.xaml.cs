using System;
using System.Collections.Generic;
using System.Linq;
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
            if (BindingContext is ExpenseDetailPageViewModel viewModel) {
                await viewModel.Save();
                //await Navigation.PopModalAsync();
                await Navigation.PopModalAsync();
            }
        }
    }
}
