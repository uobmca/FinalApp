
namespace FinalApp.Models {
    using Newtonsoft.Json;
    using Xamarin.Forms;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public partial class Category : BindableObject {
        [J("categoryId")] public long CategoryId { get; set; }
        [J("displayName")] public string DisplayName { get; set; }
        [J("icon")] public string Icon { get; set; }
    }

    public partial class Category {
        public static Category FromJson(string json) => JsonConvert.DeserializeObject<Category>(json, Commons.StandardJsonConverter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this Category self) => JsonConvert.SerializeObject(self, Commons.StandardJsonConverter.Settings);
    }
}
