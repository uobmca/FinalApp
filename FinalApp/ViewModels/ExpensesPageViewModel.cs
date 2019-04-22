using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FinalApp.Models;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace FinalApp.ViewModels {

    public class ExpensesGroupedList : List<UserExpense> { 
        public int ExpensesCategoryId { get; set; }

        public ExpensesGroupedList() { }

        public ExpensesGroupedList(int expensesCategoryId) {
            ExpensesCategoryId = expensesCategoryId;
        }

        public ExpensesGroupedList(int expensesCategoryId, List<UserExpense> expenses) {
            ExpensesCategoryId = expensesCategoryId;
            this.Clear();
            this.AddRange(expenses);
        }
    }

    public class ExpensesPageViewModel : BindableObject {

        protected static readonly BindableProperty ExpensesChartProperty =
            BindableProperty.Create(nameof(ExpensesChart), typeof(Chart), typeof(ExpensesPageViewModel), null);

        protected static readonly BindableProperty UserExpensesProperty =
            BindableProperty.Create(nameof(UserExpenses), typeof(IEnumerable<UserExpense>), typeof(ExpensesPageViewModel), new List<UserExpense>());

        protected static readonly BindableProperty GroupedUserExpensesProperty =
            BindableProperty.Create(nameof(GroupedUserExpenses), typeof(List<ExpensesGroupedList>), typeof(ExpensesPageViewModel), new List<ExpensesGroupedList>());

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

        public ExpensesPageViewModel() {
            UserExpenses = new List<UserExpense>() {
                new UserExpense {
                    Amount = 10.0,
                    CategoryId = 1
                },
                new UserExpense {
                    Amount = 22.0,
                    CategoryId = 2
                },
                new UserExpense {
                    Amount = 34.0,
                    CategoryId = 1
                },
                new UserExpense {
                    Amount = 46.0,
                    CategoryId = 2
                },
                new UserExpense {
                    Amount = 58.0,
                    CategoryId = 1
                },
                new UserExpense {
                    Amount = 70.0,
                    CategoryId = 3
                },
                new UserExpense {
                    Amount = 82.0,
                    CategoryId = 2
                },
                new UserExpense {
                    Amount = 94.0,
                    CategoryId = 1
                },
                new UserExpense {
                    Amount = 106.0,
                    CategoryId = 3
                }
            };
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == UserExpensesProperty.PropertyName) {
                UpdateExpensesChart();
            }
        }

        private void UpdateExpensesChart() {

            ExpensesChart = new RadialGaugeChart {
                AnimationDuration = TimeSpan.FromMilliseconds(600.0),
                Entries = UserExpenses.GroupBy((arg) => arg.CategoryId).Select((arg) => new ChartEntry((float)arg.First().Amount) {
                    Color = CategoryIdToSKColor((int)arg.First().CategoryId),
                    TextColor = CategoryIdToSKColor((int)arg.First().CategoryId),
                    Label = CategoryIdToString((int)arg.First().CategoryId),
                    ValueLabel = arg.First().Amount.ToString("F1")
                }), LabelTextSize = 18.0f, BackgroundColor = SKColors.Transparent
            };

            GroupedUserExpenses = UserExpenses
                .GroupBy((expense) => expense.CategoryId)
                .Select((group) => new ExpensesGroupedList((int)group.Key, group.ToList()))
                .ToList();
        }

        private string CategoryIdToString(int id) { 
            switch(id) {
                case 1: return "House";
                case 2: return "Car";
                case 3: return "Entertainment";
                default: return "Other";
            }
        }

        private SKColor CategoryIdToSKColor(int id) {
            switch (id) {
                case 1: return SKColors.Green;
                case 2: return SKColors.Blue;
                case 3: return SKColors.DeepPink;
                default: return SKColors.Red;
            }
        }

        public void Update() { 
        
        }


    }
}
