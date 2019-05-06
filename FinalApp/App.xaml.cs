using System;
using System.Threading.Tasks;
using Autofac;
using FinalApp.Network;
using FinalApp.Services;
using FinalApp.ViewModels;
using FinalApp.Views.Pages.Login;
using FinalApp.Views.Pages.MainMasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FinalApp
{
    public interface IAuthenticate {
        Task<bool> Authenticate();
    }

    public partial class App : Application {

        public static IAuthenticate Authenticator { get; private set; }
        public static void Init(IAuthenticate authenticator) {
            Authenticator = authenticator;
        }

        public static IContainer Container { get; private set; }

        public App() {
            InitializeComponent();
            RegisterDependencies();
            MainPage = new LoginPage();
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }

        private void RegisterDependencies() {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<WestEuropeOCRServices>().As<INetworkOCRServices>().SingleInstance();
            builder.RegisterType<UserDataRepository>().As<IUserDataRepository>().SingleInstance();
            builder.RegisterType<OCRDataExtractor>().As<IOCRDataExtractor>().SingleInstance();
            builder.RegisterType<AnalyzePicturePageViewModel>();
            builder.RegisterType<ExpensesPageViewModel>();
            builder.RegisterType<IncomesPageViewModel>();
            builder.RegisterType<TagsPageViewModel>();
            builder.RegisterType<TagDetailPageViewModel>().InstancePerDependency();
            builder.RegisterType<IncomeDetailPageViewModel>();
            builder.RegisterType<ExpenseDetailPageViewModel>();
            builder.RegisterType<ExpensesListPageViewModel>();
            builder.RegisterType<DashboardPageViewModel>();
            builder.RegisterType<IncomesListPageViewModel>();
            Container = builder.Build();
        }
    }
}
