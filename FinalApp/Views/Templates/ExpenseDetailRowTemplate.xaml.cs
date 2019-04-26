using System;
using System.Collections.Generic;
using FinalApp.Models;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class ExpenseDetailRowTemplate : ViewCell {
        public ExpenseDetailRowTemplate() {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is UserExpense expense) {
                dateLabel.Text = expense.ExpireDate.Date.ToShortDateString();
                amountLabel.Text = string.Format("{0:C}", expense.Amount);
            }
        }
    }
}
