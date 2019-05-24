
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using FinalApp.Commons;
using FinalApp.Network;
using Firebase.CloudMessaging;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;
using UserNotifications;

namespace FinalApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate, IUNUserNotificationCenterDelegate, IMessagingDelegate {

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            SetupLibraries();
            App.Init(this);
            LoadApplication(new App());

            UINavigationBar.Appearance.BarTintColor = new UIColor(red: 0.06f, green: 0.18f, blue:0.25f, alpha:1.0f);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes {
                TextColor = UIColor.White
            });

            SetupPushNotifications();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options) {
            return LoginManager.Instance.MobileClient.ResumeWithURL(url);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation) {
            return LoginManager.Instance.MobileClient.ResumeWithURL(url);
        }

        // IAuthenticate implementation
        public async Task<bool> Authenticate() {
            MobileServiceClient client = LoginManager.Instance.MobileClient;
            UIViewController rootVC = UIApplication.SharedApplication.KeyWindow.RootViewController;
            MobileServiceUser user = await client.LoginAsync(rootVC, MobileServiceAuthenticationProvider.Google, AppGlobalConfig.GoogleUrlScheme);

            return user != null;
        }

        private void SetupLibraries() {
            Rg.Plugins.Popup.Popup.Init();
            Firebase.Core.App.Configure();
        }

        private async void SetupPushNotifications() {
            #pragma warning disable XI0003 // Notifies you when using a deprecated, obsolete or unavailable Apple API
            #pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)) {
                // iOS 10 or later
                UNAuthorizationOptions authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => Console.WriteLine(granted));
                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
                // For iOS 10 data message (sent via FCM)
                Messaging.SharedInstance.Delegate = this;
            } else {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            var instanceId = await Firebase.InstanceID.InstanceId.SharedInstance.GetIdAsync();
            Debug.Print("Instance ID: " + instanceId);
            #pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            #pragma warning restore XI0003 // Notifies you when using a deprecated, obsolete or unavailable Apple API
        }
    }
}
