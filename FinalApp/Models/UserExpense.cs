
namespace FinalApp.Models {
    using System;
    using Newtonsoft.Json;
    using Xamarin.Forms;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class UserExpense {
        public string Id { get; set; }
        [J("amount")] public double Amount { get; set; }
        [J("description")] public string Description { get; set; }
        [J("startDate")] public DateTimeOffset StartDate { get; set; }
        [J("expireDate")] public DateTimeOffset ExpireDate { get; set; }
        [J("categoryId")] public string CategoryId { get; set; }
        [J("userId")] public string UserId { get; set; }
    }

    public partial class UserExpense {
        [JsonIgnore]
        public Category UserCategory { get; set; }
    }

    public partial class UserExpense {
        public static UserExpense FromJson(string json) => JsonConvert.DeserializeObject<UserExpense>(json, Commons.StandardJsonConverter.Settings);
    }

    public partial class UserExpense { 
        [JsonIgnore]
        public string CategoryName { 
            get {
                switch (CategoryId) {
                    case "1": return "House";
                    case "2": return "Car";
                    case "3": return "Entertainment";
                    default: return "Other";
                }
            }
        }

    }

    public class SelectableUserExpense {
        public static class BillTypes {
            public readonly static int Type01 = 0;
            public readonly static int Type02 = 1;
            public readonly static int Type03 = 2;
        }

        public static class ReceiptTypes {
            public readonly static int GenericReceipt = 3;
        }

        public ImageSource ExpenseImage { get; set; }
        public string DisplayName { get; set; }
        public int ExpenseType { get; set; }
    }


}

