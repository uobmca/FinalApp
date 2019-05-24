using System.Reflection;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.Tags {

    public partial class TagDetailPage : ContentPage {

        private CategoryIconsGridBuilder categoryIconsBuilder;

        public TagDetailPage() {
            InitializeComponent();
            SetupUI(); 
            if(BindingContext == null) { 
                SetupContext();
            }
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is TagDetailPageViewModel viewModel) {
                categoryIconsBuilder.SetSelected(viewModel.SelectedCategory);
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
            categoryIconsBuilder = new CategoryIconsGridBuilder();
            categoriesGridContainer.Children.Add(categoryIconsBuilder.Build(5));

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
                viewModel.SetCategoryIcon(categoryIconsBuilder.SelectedCategoryIcon.IconSource);
                await viewModel.Save();
                await Navigation.PopModalAsync();
            }
        }
    }
}
