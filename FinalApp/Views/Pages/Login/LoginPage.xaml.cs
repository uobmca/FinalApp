using System;
using System.Collections.Generic;
using System.Linq;
using FinalApp.Network;
using FinalApp.Views.Pages.MainMasterDetail;
using FinalApp.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.Login {
    public partial class LoginPage : ContentPage {
        public LoginPage() {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, EventArgs e) {
            if (App.Authenticator != null) {
                if (sender is Button loginButton) {
                    loginButton.IsEnabled = false;
                }
                var authenticated = await App.Authenticator.Authenticate();
                var error = true;
                if (authenticated == true) {
                    error = false;
                    var user = LoginManager.Instance.MobileClient.CurrentUser;

                    var loadingPopup = new ActivityIndicatorPopup();
                    await PopupNavigation.Instance.PushAsync(loadingPopup);
                    error = !(await LoginManager.Instance.RetrieveUserData());
                    await PopupNavigation.Instance.RemovePageAsync(loadingPopup);


                    if (sender is Button button) {
                        button.IsEnabled = true;
                    }
                }

                if (error) {
                    await DisplayAlert("Error", "Something went wrong while trying to login with your account. Please try again.", "Ok");
                } else {
                    Application.Current.MainPage = new MainMasterDetailPage();
                }
            }
        }
    }
}
