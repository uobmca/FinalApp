using System;
using System.ComponentModel;
using FinalApp.Utilities.AlertUtilities;
using FinalApp.Views;
using Xamarin.Forms;

namespace FinalApp.ViewModels
{
    public class TwoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IAlertUtility alertUtility;
        private readonly INavigation navigation;

        public Command TestTwoCommand { get; set; }
        //private PageOne pageOne;

        public TwoViewModel(IViewContext viewContext)
        {
            this.alertUtility = viewContext.GetAlertUtility();
            this.navigation = viewContext.GetNavigation();
            TestTwoCommand = new Command(OnTwoTest);
        }

        private void OnTwoTest()
        {
            alertUtility.DisplayAlertAsync("Message", "Test Page 2", "Ok");
        }
    }
}

