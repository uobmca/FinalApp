using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Media;
using System.Threading.Tasks;
using FinalApp.Network;
using Microsoft.WindowsAzure.MobileServices;
using FinalApp.Commons;

namespace FinalApp.Droid
{
    [Activity(Label = "FinalApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            SetupLibraries(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            await CrossMedia.Current.Initialize();

            App.Init((IAuthenticate) this);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task<bool> Authenticate() {
            MobileServiceClient client = LoginManager.Instance.MobileClient;
            var user = await client.LoginAsync(this, MobileServiceAuthenticationProvider.Google, AppGlobalConfig.GoogleUrlScheme);
            return user != null;
        }

        private void SetupLibraries(Bundle bundle) {
            Rg.Plugins.Popup.Popup.Init(this, bundle);
        }
    }
}