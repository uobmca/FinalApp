using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FinalApp.Models;
using Microcharts;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class IncomeExpensesPageViewModel : BindableObject {

        protected static readonly BindableProperty ExpensesChartProperty =
            BindableProperty.Create(nameof(ExpensesChart), typeof(Chart), typeof(IncomeExpensesPageViewModel), null);

        protected static readonly BindableProperty UserExpensesProperty =
            BindableProperty.Create(nameof(UserExpenses), typeof(IEnumerable<UserExpense>), typeof(IncomeExpensesPageViewModel), new List<UserExpense>());

        public RadialGaugeChart ExpensesChart {
            get => (RadialGaugeChart)GetValue(ExpensesChartProperty);
            set => SetValue(ExpensesChartProperty, value);
        }

        public IEnumerable<UserExpense> UserExpenses {
            get => (IEnumerable<UserExpense>)GetValue(UserExpensesProperty);
            set => SetValue(UserExpensesProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == UserExpensesProperty.PropertyName) {
                UpdateExpensesChart();
            }
        }

        private void UpdateExpensesChart() {
            ExpensesChart = new RadialGaugeChart {
                Entries = UserExpenses.GroupBy((arg) => arg.CategoryId).Select((arg) => new Microcharts.Entry( (float)arg.First().Amount) { 
                    Color = Color.Red, 
                    TextColor = Color.Red, 
                    Label = arg.First().CategoryId.ToString(), 
                    ValueLabel = arg.First().Amount.ToString("F1") });
        }

        public void Update() { 
        
        }


    }
}
