using System;
using System.ComponentModel;
using FinalApp.Utilities.AlertUtilities;
using Xamarin.Forms;

namespace FinalApp
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IAlertUtility alertUtility;
        private readonly INavigation navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command LogoutCommand { set; get; }

        public MainPageViewModel(IViewContext viewContext)
        {
            this.alertUtility = viewContext.GetAlertUtility();
            this.navigation = viewContext.GetNavigation();
            LogoutCommand = new Command(HandleAction);

        }

        public void HandleAction(object obj)
        {
            Application.Current.MainPage = new Login();
        }

    }
}
