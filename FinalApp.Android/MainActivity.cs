using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Plugin.CurrentActivity;
using Plugin.Media;
using System.Threading.Tasks;
using FinalApp.Network;
using Microsoft.WindowsAzure.MobileServices;
using FinalApp.Commons;
using System.Diagnostics;
using Android.OS;
using Firebase.Iid;

namespace FinalApp.Droid {

    [Activity(Label = "FinalApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {

        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "firebase_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            SetupLibraries(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            await CrossMedia.Current.Initialize();

            CreateNotificationChannel();

            //Firebase.FirebaseApp.InitializeApp(this);
            Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic("ocr_notifications");

            System.Diagnostics.Debug.Print("ANDROID FIREBASE TOKEN:" + FirebaseInstanceId.Instance.Token);

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

        void CreateNotificationChannel() {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }
            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default) {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private void SetupLibraries(Bundle bundle) {
            Rg.Plugins.Popup.Popup.Init(this, bundle);
        }
    }
}