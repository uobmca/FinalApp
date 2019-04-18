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

        protected override void OnAppearing() {
            base.OnAppearing();
            if (viewModel == null) return;
            viewModel.Update();
        }
    }
}
