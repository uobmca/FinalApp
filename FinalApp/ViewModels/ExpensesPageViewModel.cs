using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace FinalApp.ViewModels {

    public class ExpensesGroupedList : List<UserExpense> {
        public string ExpensesCategoryId { get; set; }

        public ExpensesGroupedList() { }

        public ExpensesGroupedList(string expensesCategoryId) {
            ExpensesCategoryId = expensesCategoryId;
        }

        public ExpensesGroupedList(string expensesCategoryId, List<UserExpense> expenses) {
            ExpensesCategoryId = expensesCategoryId;
            this.Clear();
            this.AddRange(expenses);
        }
    }

    public class ExpensesPageViewModel : BindableObject {

        protected readonly BindableProperty ExpensesChartProperty =
            BindableProperty.Create(nameof(ExpensesChart), typeof(Chart), typeof(ExpensesPageViewModel), null);

        protected readonly BindableProperty UserExpensesProperty =
            BindableProperty.Create(nameof(UserExpenses), typeof(IEnumerable<UserExpense>), typeof(ExpensesPageViewModel), new List<UserExpense>());

        protected readonly BindableProperty GroupedUserExpensesProperty =
            BindableProperty.Create(nameof(GroupedUserExpenses), typeof(List<ExpensesGroupedList>), typeof(ExpensesPageViewModel), new List<ExpensesGroupedList>());

        protected readonly BindableProperty ExpensesBalanceProperty =
            BindableProperty.Create(nameof(ExpensesBalance), typeof(string), typeof(ExpensesPageViewModel), "-");

        protected readonly BindableProperty AverageExpenseProperty =
            BindableProperty.Create(nameof(AverageExpense), typeof(string), typeof(ExpensesPageViewModel), "-");

        protected readonly BindableProperty BusiestDayProperty =
            BindableProperty.Create(nameof(BusiestDay), typeof(string), typeof(ExpensesPageViewModel), "-");

        public RadialGaugeChart ExpensesChart {
            get => (RadialGaugeChart)GetValue(ExpensesChartProperty);
            set => SetValue(ExpensesChartProperty, value);
        }

        public IEnumerable<UserExpense> UserExpenses {
            get => (IEnumerable<UserExpense>)GetValue(UserExpensesProperty);
            set => SetValue(UserExpensesProperty, value);
        }

        public List<ExpensesGroupedList> GroupedUserExpenses {
            get => (List<ExpensesGroupedList>)GetValue(GroupedUserExpensesProperty);
            set => SetValue(GroupedUserExpensesProperty, value);
        }

        public string ExpensesBalance {
            get => (string)GetValue(ExpensesBalanceProperty);
            set => SetValue(ExpensesBalanceProperty, value);
        }

        public string AverageExpense {
            get => (string)GetValue(AverageExpenseProperty);
            set => SetValue(AverageExpenseProperty, value);
        }

        public string BusiestDay {
            get => (string)GetValue(BusiestDayProperty);
            set => SetValue(BusiestDayProperty, value);
        }

        private IUserDataRepository repository;
        private IEnumerable<Category> userCategories;
        private List<UserIncome> userIncomes;

        public ExpensesPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
            Update();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == UserExpensesProperty.PropertyName) {
                UpdateOverview();
                UpdateExpensesChart();
            }
        }

        private async void UpdateOverview() {

            // Balance
            double expensesSum = UserExpenses.Sum((exp) => exp.Amount);
            ExpensesBalance = string.Format("{0:C}", Math.Abs(expensesSum));

            // Average
            AverageExpense = string.Format("- {0:C}", expensesSum / (double)UserExpenses.Count());

            // Busiest day
            var groupedExpenses = UserExpenses.GroupBy((exp) => {
                DateTimeOffset date = exp.StartDate;
                if (exp.ExpireDate != default(DateTimeOffset)) {
                    date = exp.ExpireDate;
                }
                return date.DayOfWeek;
            });

            DayOfWeek busiestDay = DayOfWeek.Monday;
            double busiestDayTotal = double.MinValue;
            foreach(var group in groupedExpenses) {
                var totalInGroup = group.Sum((exp) => exp.Amount);
                if (totalInGroup > busiestDayTotal) {
                    busiestDayTotal = totalInGroup;
                    busiestDay = group.Key;
                }
            }

            BusiestDay = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(busiestDay);
        }

        // private async void UpdateOverviewOptimized() {
        //     double balance = (await repository.GetUserIncomes().Sum((inc) => inc.Amount)) - UserExpenses.Sum((exp) => exp.Amount);
        //     ExpensesBalance = string.Format("{0} {1:C}", balance == 0 ? "" : (balance > 0 ? "+" : "-"), balance);
        // }

        private void UpdateExpensesChart() {

            ExpensesChart = new RadialGaugeChart {
                AnimationDuration = TimeSpan.FromMilliseconds(600.0),
                Entries = UserExpenses.GroupBy((arg) => arg.CategoryId).Select((arg) => new ChartEntry((float)arg.First().Amount) {
                    Color = CategoryIdToSKColor(arg.First().CategoryId),
                    TextColor = CategoryIdToSKColor(arg.First().CategoryId),
                    Label = CategoryIdToString(arg.First().CategoryId),
                    ValueLabel = arg.First().Amount.ToString("F1")
                }),
                LabelTextSize = 18.0f,
                BackgroundColor = SKColors.Transparent
            };

            GroupedUserExpenses = UserExpenses
                .GroupBy((expense) => expense.CategoryId)
                .Select((group) => new ExpensesGroupedList(group.Key, group.ToList()))
                .ToList();
        }

        private string CategoryIdToString(string id) {
            var category = userCategories.FirstOrDefault((arg) => arg.Id == id);
            if (category != null) {
                return category.DisplayName;
            }
            return "";
        }

        private SKColor CategoryIdToSKColor(string id) {
            switch (id) {
                case "1": return SKColors.Green;
                case "2": return SKColors.Blue;
                case "3": return SKColors.DeepPink;
                default: return SKColors.Red;
            }
        }

        public async Task Update() {
            userCategories = await repository.GetUserCategories();
            UserExpenses = await repository.GetUserExpenses();

            foreach (UserExpense expense in UserExpenses) {
                expense.UserCategory = userCategories.FirstOrDefault((category) => category.Id == expense.CategoryId);
            }
            UpdateOverview();
            UpdateExpensesChart();
        }


    }
}
