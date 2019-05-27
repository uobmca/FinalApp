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

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if(BindingContext is ExpensesListPageViewModel viewModel) { 
                viewModel.DeleteCommand = new Command<UserExpense>(async (userExpense) => {
                    bool userHasConfirmed = await DisplayAlert("Delete expense", "Are you sure you want to delete this expense?", "Yes", "No");
                    if (userHasConfirmed) {
                        await viewModel.Remove(userExpense);
                    }
                });

            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {

        }
    }
}
