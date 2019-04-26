
namespace FinalApp.Models {
    using System;
    using Newtonsoft.Json;
    using Xamarin.Forms;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public partial class UserIncome : BindableObject {
        public string Id { get; set; }
        [J("amount")] public double Amount { get; set; }
        [J("description")] public string Description { get; set; }
        [J("incomeDate")] public DateTimeOffset IncomeDate { get; set; }
        [J("categoryId")] public long CategoryId { get; set; }
        [J("createdAt")] public DateTimeOffset CreatedAt { get; set; }
        [J("updatedAt")] public DateTimeOffset UpdatedAt { get; set; }
    }

    public partial class UserIncome {
        public static UserIncome FromJson(string json) => JsonConvert.DeserializeObject<UserIncome>(json, Commons.StandardJsonConverter.Settings);
    }

}
