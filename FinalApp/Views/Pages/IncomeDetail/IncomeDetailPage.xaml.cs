using System;
using System.Collections.Generic;
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

            using(var scope = App.Container.BeginLifetimeScope()) { 
                if (scope.Resolve<IncomeDetailPageViewModel>() is IncomeDetailPageViewModel viewModel) {
                    BindingContext = viewModel;
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
