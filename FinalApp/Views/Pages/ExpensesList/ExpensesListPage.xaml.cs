using System;
using System.Collections.Generic;
using Autofac;
using FinalApp.Models;
using FinalApp.ViewModels;
using FinalApp.Views.Base;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.ExpensesList {
    public partial class ExpensesListPage : ContentPage {
        public ExpensesListPage() {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", async () => {
                await Navigation.PopModalAsync();
            }));
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {

        }
    }
}
