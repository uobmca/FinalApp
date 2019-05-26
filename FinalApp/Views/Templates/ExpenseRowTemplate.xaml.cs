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
                if(groupedExpenses.FirstOrDefault() is UserExpense expense) { 
                    if (expense.UserCategory is Category category) {
                        categoryLabel.Text = category.DisplayName ?? "N/A";
                        categoryIconImage.Source = category.Icon ?? "ic_home";
                    }
                }
                transactionsLabel.Text = $"{groupedExpenses.Count} transactions";
                amountLabel.Text = string.Format("- {0:C}", groupedExpenses.Sum((exp) => exp.Amount));
            }
        }
    }
}
