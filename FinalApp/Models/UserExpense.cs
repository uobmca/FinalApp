
namespace FinalApp.Models {
    using System;
    using Newtonsoft.Json;
    using Xamarin.Forms;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public partial class UserExpense : BindableObject {
        [J("amount")] public double Amount { get; set; }
        [J("description")] public string Description { get; set; }
        [J("startDate")] public DateTimeOffset StartDate { get; set; }
        [J("expireDate")] public DateTimeOffset ExpireDate { get; set; }
        [J("categoryId")] public long CategoryId { get; set; }
        [J("createdAt")] public DateTimeOffset CreatedAt { get; set; }
        [J("updatedAt")] public DateTimeOffset UpdatedAt { get; set; }
    }

    public partial class UserExpense {
        public static UserExpense FromJson(string json) => JsonConvert.DeserializeObject<UserExpense>(json, Commons.StandardJsonConverter.Settings);
    }

    public partial class UserExpense { 
        public string CategoryName { 
            get {
                switch (CategoryId) {
                    case 1: return "House";
                    case 2: return "Car";
                    case 3: return "Entertainment";
                    default: return "Other";
                }
            }
        }

    }


}

