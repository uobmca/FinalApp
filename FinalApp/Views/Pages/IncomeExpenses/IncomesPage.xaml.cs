using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomeExpenses {
    public partial class IncomesPage : ContentPage {

        private IncomesPageViewModel viewModel;

        public IncomesPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (DesignMode.IsDesignModeEnabled) return;
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<IncomesPageViewModel>() is IncomesPageViewModel viewModel) {
                    BindingContext = viewModel;
                    this.viewModel = viewModel;
                }
            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {
            if (e.SelectedItem is ExpensesGroupedList expenseList) {
                using (var scope = App.Container.BeginLifetimeScope()) {
                    if (scope.Resolve<ExpensesListPageViewModel>() is ExpensesListPageViewModel viewModel) {
                        //var page = new IncomesLis();
                        //viewModel.CategoryId = expenseList.ExpensesCategoryId;
                        //page.BindingContext = viewModel;
                        //Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
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
