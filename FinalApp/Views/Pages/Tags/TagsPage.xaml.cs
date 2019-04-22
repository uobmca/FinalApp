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
            CategoryEditButtonCommand = new Command<Models.Category>(async (category) => {
                if (App.Container.Resolve<TagDetailPageViewModel>() is TagDetailPageViewModel vm) {
                    vm.SelectedCategory = category;
                    var page = new TagDetailPage {
                        BindingContext = vm
                    };
                    await Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
                }
            });

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
            await Navigation.PushModalAsyncUnique(new AppNavigationPage(new TagDetailPage()));
            if (BindingContext  is TagsPageViewModel viewModel) {
                await viewModel.ReloadData();
            }
        }
    }
}
