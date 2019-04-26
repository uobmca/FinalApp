using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FinalApp.Commons;
using FinalApp.Models;
using FinalApp.ViewModels;
using FinalApp.Views.Base;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.Tags {
    public partial class TagsPage : ContentPage {

        protected static readonly BindableProperty CategoryEditButtonCommandProperty =
            BindableProperty.Create(nameof(CategoryEditButtonCommand), typeof(Command<Category>), typeof(TagsPageViewModel), null);
            
        public Command<Category> CategoryEditButtonCommand {
            get => (Command<Category>)GetValue(CategoryEditButtonCommandProperty);
            set => SetValue(CategoryEditButtonCommandProperty, value);
        }

        public TagsPage() {
            InitializeComponent();
            SetupUI();

            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<TagsPageViewModel>() is TagsPageViewModel viewModel) {
                    BindingContext = viewModel;
                    viewModel.ReloadData();

                    CategoryEditButtonCommand = new Command<Models.Category>(async (category) => {
                        var vm = new TagDetailPageViewModel(viewModel.repository) {
                            SelectedCategory = category
                        };
                        var page = new TagDetailPage {
                            BindingContext = vm
                        };
                        await Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
                    });
                }
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            if (App.Container.Resolve<TagsPageViewModel>() is TagsPageViewModel viewModel) {
                BindingContext = viewModel;
                viewModel.ReloadData();
            }
        }

        private void SetupUI() {
            ToolbarItems.Add(new ToolbarItem("Add", "ic_add", () => {
                GoToTagDetail();
            }));
        }

        private async Task GoToTagDetail() {
            if (BindingContext  is TagsPageViewModel viewModel) {
                await Navigation.PushModalAsyncUnique(new AppNavigationPage(new TagDetailPage() { 
                    BindingContext = new TagDetailPageViewModel(viewModel.repository)
                }));
                await viewModel.ReloadData();
            }
        }
    }
}
