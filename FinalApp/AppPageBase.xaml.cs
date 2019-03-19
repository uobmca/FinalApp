using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinalApp.Utilities.AlertUtilities;
using Xamarin.Forms;

namespace FinalApp
{
    public partial class AppPageBase : ContentPage, IAlertUtility,IViewContext 
    {
        public AppPageBase()
        {
            InitializeComponent();
        }

        public Task DisplayAlertAsync(string title, string message, string cancel)
        {
            return DisplayAlert(title, message, cancel);
        }

        public IAlertUtility GetAlertUtility()
        {
            return this;
        }

        public INavigation GetNavigation()
        {
            return this.Navigation;
        }
    }
}
