
namespace FinalApp.Models {
    using System;
    using Newtonsoft.Json;
    using Xamarin.Forms;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class UserIncome {
        [J("id")] public string Id { get; set; }
        [J("amount")] public double Amount { get; set; }
        [J("description")] public string Description { get; set; }
        [J("incomeDate")] public DateTimeOffset IncomeDate { get; set; }
        [J("categoryId")] public string CategoryId { get; set; }
        [J("userId")] public string UserId { get; set; }
    }

    public partial class UserIncome {
        public Category UserCategory { get; set; }
    }

    public partial class UserIncome {
        public static UserIncome FromJson(string json) => JsonConvert.DeserializeObject<UserIncome>(json, Commons.StandardJsonConverter.Settings);
    }

}
