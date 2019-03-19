using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FinalApp.Utilities.AlertUtilities;
using FinalApp.Views;
using Xamarin.Forms;

namespace FinalApp.ViewModels
{
    public class OneVIewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IAlertUtility alertUtility;
        private readonly INavigation navigation;

        public Command TestCommand { set; get; }

        public OneVIewModel(IViewContext viewContext)
        {
            this.alertUtility = viewContext.GetAlertUtility();
            this.navigation = viewContext.GetNavigation();
            TestCommand = new Command(async () => await OnTest());
        }

        private async Task OnTest()
        {
            //alertUtility.DisplayAlertAsync("Error", "Invalid Login, try again", "OK");
            await navigation.PushAsync(new MyPageNav());
        }

       
    }
}
