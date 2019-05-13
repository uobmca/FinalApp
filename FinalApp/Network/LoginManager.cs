using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinalApp.Commons;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace FinalApp.Network {
    public class LoginManager {

        // Singleton default initializer
        private static LoginManager instance;
        public static LoginManager Instance {
            get {
                if (instance == null) {
                    instance = new LoginManager();
                }
                return instance;
            }
        }

        // Public properties
        public MobileServiceUser LoggedUser { get; set; }
        public MobileServiceClient MobileClient { get; private set; }
        public string UserName { get; private set; }
        public string UserEmail { get; private set; }
        public string UserPicture { get; private set; }

        // Base initializer
        public LoginManager() {
            MobileClient = new MobileServiceClient(AppGlobalConfig.AzureApplicationUrl);
        }

        public async Task<bool> RetrieveUserData() {
            var info = await MobileClient.InvokeApiAsync<List<SocialLoginResult>>("/.auth/me");
            if (info.FirstOrDefault() is SocialLoginResult result) {
                UserName = result.GetUserName();
                UserEmail = result.GetUserEmail();
                UserPicture = result.GetUserPictureURL();
                return true;
            }
            return false;
        }

        public async Task<List<SocialLoginResult>> GetUserData() {
            return await MobileClient.InvokeApiAsync<List<SocialLoginResult>>("/.auth/me");
        }

    }
}

