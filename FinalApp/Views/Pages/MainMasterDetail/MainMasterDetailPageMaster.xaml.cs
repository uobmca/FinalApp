using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalApp.Views.Pages.MainMasterDetail {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPageMaster : ContentPage {
        public ListView ListView;

        public MainMasterDetailPageMaster() {
            InitializeComponent();

            BindingContext = new MainMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMasterDetailPageMasterViewModel : INotifyPropertyChanged {
            public ObservableCollection<MainMasterDetailPageMenuItem> MenuItems { get; set; }

            public MainMasterDetailPageMasterViewModel() {
                MenuItems = new ObservableCollection<MainMasterDetailPageMenuItem>(new[]
                {
                    new MainMasterDetailPageMenuItem { Id = 0, Icon = "ic_drawer_dashboard", Title = "Dashboard", TargetType = typeof(Dashboard.DashboardPage) },
                    new MainMasterDetailPageMenuItem { Id = 1, Icon = "ic_drawer_money", Title = "Income / Expenses", TargetType = typeof(IncomeExpenses.IncomeExpensesPage) },
                    new MainMasterDetailPageMenuItem { Id = 2, Icon = "ic_drawer_report", Title = "Reports" }
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
