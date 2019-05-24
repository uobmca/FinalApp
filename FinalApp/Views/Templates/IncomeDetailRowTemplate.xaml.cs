using System;
using System.Collections.Generic;
using FinalApp.Models;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class IncomeDetailRowTemplate : ViewCell {
        public IncomeDetailRowTemplate() {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is UserIncome income) {
                dateLabel.Text = income.IncomeDate.Date.ToShortDateString();
                amountLabel.Text = string.Format("{0:C}", income.Amount);
                categoryIconImage.Source = income.UserCategory != null ? (income.UserCategory.Icon != "" ? income.UserCategory.Icon : "ic_tag_home") : "ic_tag_home";
            }
        }
    }
}
