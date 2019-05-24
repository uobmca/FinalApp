using System;
using System.Collections.Generic;
using System.Linq;
using FinalApp.Models;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class ExpenseRowTemplate : ViewCell {
        public ExpenseRowTemplate() {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is ExpensesGroupedList groupedExpenses) {
                categoryLabel.Text = groupedExpenses.First().UserCategory != null ? groupedExpenses.First().UserCategory.DisplayName : "N/A";
                transactionsLabel.Text = $"{groupedExpenses.Count} transactions";
                amountLabel.Text = string.Format("- {0:C}", groupedExpenses.Sum((expense) => expense.Amount));
                categoryIconImage.Source = groupedExpenses.First().UserCategory.Icon != "" ? groupedExpenses.First().UserCategory.Icon : "ic_home";
            }
        }
    }
}
