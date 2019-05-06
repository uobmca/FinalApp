using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FinalApp.Network {
    public partial class SocialLoginResult {
        [JsonProperty(PropertyName = "id_token")]
        public string IdToken { get; set; }

        [JsonProperty(PropertyName = "provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "user_claims")]
        public List<UserClaim> UserClaims { get; set; }
    }

    public class UserClaim {
        [JsonProperty(PropertyName = "typ")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "val")]
        public string Value { get; set; }
    }

    public partial class SocialLoginResult {

        public string GetUserName() {
            var claim = UserClaims.FirstOrDefault((c) => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));
            if (claim == null) {
                claim = UserClaims.FirstOrDefault((c) => c.Type.Equals("name"));
            }
            if (claim == null) {
                return "";
            }
            return claim.Value;
        }

        public string GetUserEmail() {
            var claim = UserClaims.FirstOrDefault((c) => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"));
            if (claim == null) {
                claim = UserClaims.FirstOrDefault((c) => c.Type.Equals("emailaddress"));
            }
            if (claim == null) {
                return "";
            }
            return claim.Value;
        }

        public string GetUserPictureURL() {
            var claim = UserClaims.FirstOrDefault((c) => c.Type.Equals("picture"));
            if (claim == null) {
                return "";
            }
            return claim.Value;
        }

    }
}