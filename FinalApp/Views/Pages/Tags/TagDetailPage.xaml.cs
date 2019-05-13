using System;
using System.Collections.Generic;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.Tags {
    public partial class TagDetailPage : ContentPage {
        public TagDetailPage() {
            InitializeComponent();
            SetupUI();
            if(BindingContext == null) { 
                SetupContext();
            }
        }

        private void SetupContext() { 
            using (var scope = App.Container.BeginLifetimeScope()) { 
                if(scope.Resolve<TagDetailPageViewModel>() is TagDetailPageViewModel viewModel) {
                    BindingContext = viewModel;
                }
            }
        }

        private void SetupUI() {
            NavigationPage.SetHasNavigationBar(this, true);
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", async () => {
                await Navigation.PopModalAsync();
            }));
        }

        async void Handle_Clicked(object sender, System.EventArgs e) {

            if(categoryEntry.Text == null || categoryEntry.Text.Trim().Equals("")) {
                await DisplayAlert("Error", "Insert a name for the tag to proceed", "Ok");
                return;
            }

            if(BindingContext is TagDetailPageViewModel viewModel) {
                await viewModel.Save();
                await Navigation.PopModalAsync();
            }
        }
    }
}
