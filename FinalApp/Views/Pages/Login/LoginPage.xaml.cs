using System;
using System.Collections.Generic;
using System.Linq;
using FinalApp.Network;
using FinalApp.Views.Pages.MainMasterDetail;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.Login {
    public partial class LoginPage : ContentPage {
        public LoginPage() {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, EventArgs e) {
            if (App.Authenticator != null) {
                var authenticated = await App.Authenticator.Authenticate();
                if (authenticated == true) {

                    var user = LoginManager.Instance.MobileClient.CurrentUser;
                    await LoginManager.Instance.RetrieveUserData();

                    Application.Current.MainPage = new MainMasterDetailPage();
                }
            }
        }
    }
}
