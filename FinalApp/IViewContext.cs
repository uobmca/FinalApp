using System;
using FinalApp.Utilities.AlertUtilities;
using Xamarin.Forms;

namespace FinalApp
{
    public interface IViewContext
    {
        INavigation GetNavigation();
        IAlertUtility GetAlertUtility();
    }
}
