
namespace FinalApp.Models {
    using System;
    using Newtonsoft.Json;
    using Xamarin.Forms;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class Category {
        public string Id { get; set; }
        //[J("categoryId")] public string CategoryId { get; set; }
        [J("displayName")] public string DisplayName { get; set; }
        [J("icon", NullValueHandling = N.Ignore)] public string Icon { get; set; }
        [J("userId")] public string UserId { get; set; }

        public override string ToString() {
            return DisplayName;
        }

    }

    public partial class Category {
        public static Category FromJson(string json) => JsonConvert.DeserializeObject<Category>(json, Commons.StandardJsonConverter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this Category self) => JsonConvert.SerializeObject(self, Commons.StandardJsonConverter.Settings);
    }
}
