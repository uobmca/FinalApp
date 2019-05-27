using System;
using System.Collections.Generic;
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

    public class IncomesGroupedList : List<UserIncome> {
        public string IncomesCategoryId { get; set; }

        public IncomesGroupedList() { }

        public IncomesGroupedList(string incomesCategoryId) {
            IncomesCategoryId = incomesCategoryId;
        }

        public IncomesGroupedList(string incomesCategoryId, List<UserIncome> incomes) {
            IncomesCategoryId = incomesCategoryId;
            Clear();
            AddRange(incomes);
        }
    }

    public class IncomesPageViewModel : BindableObject {

        protected readonly BindableProperty IncomesChartProperty =
            BindableProperty.Create(nameof(IncomesChart), typeof(Chart), typeof(IncomesPageViewModel), null);

        protected readonly BindableProperty UserIncomesProperty =
            BindableProperty.Create(nameof(UserIncomes), typeof(IEnumerable<UserIncome>), typeof(IncomesPageViewModel), new List<UserIncome>());

        protected readonly BindableProperty GroupedUserIncomesProperty =
            BindableProperty.Create(nameof(GroupedUserIncomes), typeof(List<IncomesGroupedList>), typeof(IncomesPageViewModel), new List<IncomesGroupedList>());

        protected readonly BindableProperty IncomesBalanceProperty =
            BindableProperty.Create(nameof(IncomesBalance), typeof(string), typeof(IncomesPageViewModel), "-");

        protected readonly BindableProperty AverageIncomeProperty =
            BindableProperty.Create(nameof(AverageIncome), typeof(string), typeof(IncomesPageViewModel), "-");

        protected readonly BindableProperty BusiestDayProperty =
            BindableProperty.Create(nameof(BusiestDay), typeof(string), typeof(IncomesPageViewModel), "-");

        public Chart IncomesChart {
            get => (Chart)GetValue(IncomesChartProperty);
            set => SetValue(IncomesChartProperty, value);
        }

        public IEnumerable<UserIncome> UserIncomes {
            get => (IEnumerable<UserIncome>)GetValue(UserIncomesProperty);
            set => SetValue(UserIncomesProperty, value);
        }

        public List<IncomesGroupedList> GroupedUserIncomes {
            get => (List<IncomesGroupedList>)GetValue(GroupedUserIncomesProperty);
            set => SetValue(GroupedUserIncomesProperty, value);
        }

        public string IncomesBalance {
            get => (string)GetValue(IncomesBalanceProperty);
            set => SetValue(IncomesBalanceProperty, value);
        }

        public string AverageIncome {
            get => (string)GetValue(AverageIncomeProperty);
            set => SetValue(AverageIncomeProperty, value);
        }

        public string BusiestDay {
            get => (string)GetValue(BusiestDayProperty);
            set => SetValue(BusiestDayProperty, value);
        }

        private IUserDataRepository repository;
        private IEnumerable<Category> userCategories;
        private List<UserExpense> userExpenses;

        private SKColor[] chartColors = new SKColor[] {
            SKColors.Blue,
            SKColors.Green,
            SKColors.Yellow,
            SKColors.Orange,
            SKColors.Firebrick,
            SKColors.MidnightBlue,
            SKColors.Turquoise,
            SKColors.Purple,
            SKColors.CornflowerBlue,
            SKColors.PaleVioletRed,
            SKColors.LimeGreen,
            SKColors.DarkSeaGreen
        };

        public IncomesPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
            Update();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == UserIncomesProperty.PropertyName) {
                UpdateOverview();
                UpdateIncomesChart();
            }
        }

        private async Task UpdateOverview() {

            // Balance
            double incomesSum = UserIncomes.Sum((exp) => exp.Amount);
            IncomesBalance = string.Format("{0:C}", Math.Abs(incomesSum));

            // Average
            AverageIncome = string.Format("{0:C}", incomesSum / UserIncomes.Count());

            // Busiest day
            var groupedIncomes = UserIncomes.GroupBy((inc) => {
                return inc.IncomeDate.DayOfWeek;
            });

            DayOfWeek busiestDay = DayOfWeek.Monday;
            double busiestDayTotal = double.MinValue;
            foreach (var group in groupedIncomes) {
                var totalInGroup = group.Sum((inc) => inc.Amount);
                if (totalInGroup > busiestDayTotal) {
                    busiestDayTotal = totalInGroup;
                    busiestDay = group.Key;
                }
            }

            var dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(busiestDay);
            var capitalizedDayName = dayName.Substring(0, 1).ToUpper() + dayName.Substring(1);
            BusiestDay = capitalizedDayName;
        }

        private void UpdateIncomesChart() {

            int colorIndex = 0;
            Dictionary<string, SKColor> assignedColors = new Dictionary<string, SKColor>();
            IncomesChart = new PieChart {
                AnimationDuration = TimeSpan.FromMilliseconds(600.0),
                Entries = UserIncomes.GroupBy((arg) => arg.CategoryId).Select((arg) => {
                    SKColor color;
                    var categoryId = arg.First().CategoryId ?? "";
                    if (assignedColors.ContainsKey(categoryId)) {
                        color = assignedColors[categoryId];
                    } else {
                        color = chartColors[colorIndex % (chartColors.Length - 1)];
                        assignedColors[categoryId] = color;
                        colorIndex++;
                    }

                    return new ChartEntry((float)arg.First().Amount) {
                        Color = color,
                        TextColor = CategoryIdToSKColor(arg.First().CategoryId),
                        Label = CategoryIdToString(arg.First().CategoryId),
                        ValueLabel = arg.First().Amount.ToString("F1")
                    };
                }),
                LabelTextSize = 18.0f,
                BackgroundColor = SKColors.Transparent
            };

            GroupedUserIncomes = UserIncomes
                .GroupBy((income) => income.CategoryId)
                .Select((group) => new IncomesGroupedList(group.Key, group.ToList()))
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

        public async void Update() {
            userCategories = await repository.GetUserCategories();

            var startDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, 0, 0, 0);
            var endDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day, 23, 59, 59);
            UserIncomes = await repository.GetUserIncomes(startDate, endDate);
            foreach (UserIncome income in UserIncomes) {
                income.UserCategory = userCategories.FirstOrDefault((category) => category.Id == income.CategoryId);
            }
            UpdateIncomesChart();
            UpdateOverview();
        }
    }
}
