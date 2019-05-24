using System;
using System.Collections.Generic;
using System.Linq;
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
                categoryLabel.Text = groupedIncomes.First().UserCategory != null ? groupedIncomes.First().UserCategory.DisplayName : "N/A";
                transactionsLabel.Text = $"{groupedIncomes.Count} transactions";
                amountLabel.Text = string.Format("{0:C}", groupedIncomes.Sum((expense) => expense.Amount));
                categoryIconImage.Source = groupedIncomes.First().UserCategory.Icon != "" ? groupedIncomes.First().UserCategory.Icon : "ic_home";
            }
        }
    }
}
