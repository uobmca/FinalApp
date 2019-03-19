using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FinalApp.Utilities.AlertUtilities
{
    public interface IAlertUtility
    {
        Task DisplayAlertAsync(string title, string message,string cancel);
    }
}
