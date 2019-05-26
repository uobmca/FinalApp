using System;
using System.Collections.Generic;
using System.Linq;
using FinalApp.Models;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class IncomeRowTemplate : ViewCell {
        public IncomeRowTemplate() {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is IncomesGroupedList groupedIncomes) {
                if(groupedIncomes.FirstOrDefault() is UserIncome income) { 
                    if(income.UserCategory is Category category) {
                        categoryLabel.Text = category.DisplayName ?? "N/A";
                        categoryIconImage.Source = category.Icon ?? "ic_home";
                    }
                }
                transactionsLabel.Text = $"{groupedIncomes.Count} transactions";
                amountLabel.Text = string.Format("{0:C}", groupedIncomes.Sum((expense) => expense.Amount));
            }
        }
    }
}
