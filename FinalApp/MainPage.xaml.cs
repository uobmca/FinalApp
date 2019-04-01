using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalApp.MenuItem;
using FinalApp.Utilities.AlertUtilities;
using FinalApp.ViewModels;
using FinalApp.Views;
using Xamarin.Forms;

namespace FinalApp
{
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }

        public MainPage()
        {
            InitializeComponent();

            menuList = new List<MasterPageItem>();

            var page1Instance = new PageOne();
            var page2Instance = new PageTwo();
            var page1 = new MasterPageItem() { Title = "Item 1", Icon = "itemIcon1.png", TargetType = page1Instance, ViewModel = new OneVIewModel(page1Instance) };
            var page2 = new MasterPageItem() { Title = "Item 2", Icon = "itemIcon2.png", TargetType = page2Instance, ViewModel = new TwoViewModel(page2Instance) };
           

            menuList.Add(page1);
            menuList.Add(page2);


            navigationDrawerList.ItemsSource = menuList;

            Detail = new NavigationPage((Page)page1.TargetType);
            Detail.BindingContext = page1.ViewModel;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = (MasterPageItem)e.SelectedItem;
            Page page =(Page)item.TargetType;
            page.BindingContext = item.ViewModel;
            Detail = new NavigationPage(page);
            IsPresented = false;
        }
    }
}
