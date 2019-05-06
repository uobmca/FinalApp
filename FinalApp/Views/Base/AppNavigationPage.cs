using System;
using Xamarin.Forms;

namespace FinalApp.Views.Base {
    public class AppNavigationPage : NavigationPage {
        public AppNavigationPage() {
            BarTextColor = Color.White;
        }

        public AppNavigationPage(Page root) : base(root) {
            BarTextColor = Color.White;
        }
    }
}
