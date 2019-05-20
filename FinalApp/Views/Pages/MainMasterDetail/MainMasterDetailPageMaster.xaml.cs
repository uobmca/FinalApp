using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinalApp.Network;
using FinalApp.Views.Pages.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalApp.Views.Pages.MainMasterDetail {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPageMaster : ContentPage {
        public ListView ListView;

        public MainMasterDetailPageMaster() {
            InitializeComponent();
            SetupUI();
            BindingContext = new MainMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            var lm = LoginManager.Instance;
            profileImage.Source = lm.UserPicture;
            userNameLabel.Text = lm.UserName;
            userEmailLabel.Text = lm.UserEmail;
        }

        private void SetupUI() {
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) => {
                await LoginManager.Instance.MobileClient.LogoutAsync();
                LoginManager.Instance.LoggedUser = null;
                Application.Current.MainPage = new LoginPage();
            };
            signOutLayout.GestureRecognizers.Add(tapGestureRecognizer);
        }

        class MainMasterDetailPageMasterViewModel : INotifyPropertyChanged {
            public ObservableCollection<MainMasterDetailPageMenuItem> MenuItems { get; set; }

            public MainMasterDetailPageMasterViewModel() {
                Debug.Print("Ok.");
                MenuItems = new ObservableCollection<MainMasterDetailPageMenuItem>(new[]
                {
                    new MainMasterDetailPageMenuItem { Id = 0, Icon = "ic_drawer_dashboard", Title = "Dashboard", TargetType = typeof(Dashboard.DashboardPage) },
                    new MainMasterDetailPageMenuItem { Id = 1, Icon = "ic_drawer_money", Title = "Income / Expenses", TargetType = typeof(IncomeExpenses.IncomesExpensesTabbedPage) },
                    new MainMasterDetailPageMenuItem { Id = 2, Icon = "ic_drawer_report", Title = "Reports", TargetType = typeof(Reports.ReportsPage) },
                    new MainMasterDetailPageMenuItem { Id = 3, Icon = "ic_drawer_tag", Title = "Tags", TargetType = typeof(Tags.TagsPage)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "") {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
