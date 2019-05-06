using System.Threading.Tasks;
using Autofac;
using FinalApp.Commons;
using FinalApp.ViewModels;
using FinalApp.Views.Base;
using FinalApp.Views.Pages.IncomesList;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomeExpenses {
    public partial class IncomesPage : ContentPage {

        private IncomesPageViewModel viewModel;

        public IncomesPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            ToolbarItems.Add(new ToolbarItem("Add", "ic_add", async () => {
                var page = new IncomeDetail.IncomeDetailPage();
                await Navigation.PushModalAsync(new AppNavigationPage(page));
            }));

            if (DesignMode.IsDesignModeEnabled) return;
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<IncomesPageViewModel>() is IncomesPageViewModel viewModel) {
                    BindingContext = viewModel;
                    this.viewModel = viewModel;
                }
            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {
            if (e.SelectedItem is IncomesGroupedList incomesList) {
                using (var scope = App.Container.BeginLifetimeScope()) {
                    if (scope.Resolve<IncomesListPageViewModel>() is IncomesListPageViewModel viewModel) {
                        var page = new IncomesListPage();
                        viewModel.CategoryId = incomesList.IncomesCategoryId;
                        page.BindingContext = viewModel;
                        Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
                    }
                }
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            if (viewModel == null) return;
            viewModel.Update();
        }
    }
}
