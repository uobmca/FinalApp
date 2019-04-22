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
            SetupContext();
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
    }
}
