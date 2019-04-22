using System;
using Xamarin.Forms;

namespace FinalApp.Views.Base {
    public class AppNavigationPage : NavigationPage {
        public AppNavigationPage() {
        }

        public AppNavigationPage(Page root) : base(root) { }
    }
}
