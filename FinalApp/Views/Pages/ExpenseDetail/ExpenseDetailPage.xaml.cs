using System;
using System.Collections.Generic;
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

            using(var scope = App.Container.BeginLifetimeScope()) { 
                if(scope.Resolve<ExpenseDetailPageViewModel>() is ExpenseDetailPageViewModel viewModel) {
                    BindingContext = viewModel;
                    viewModel.Update();
                }
            }
         }

        async void Handle_Clicked(object sender, System.EventArgs e) {
            if (BindingContext is ExpenseDetailPageViewModel viewModel) {
                await viewModel.Save();
                await Navigation.PopModalAsync();
            }
        }
    }
}
