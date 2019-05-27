using System;
using System.Collections.Generic;
using FinalApp.Models;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomesList {
    public partial class IncomesListPage : ContentPage {
        public IncomesListPage() {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", async () => {
                await Navigation.PopModalAsync();
            }));
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if(BindingContext is IncomesListPageViewModel viewModel) { 
                viewModel.DeleteCommand = new Command<UserIncome>(async (userIncome) => {
                    bool userHasConfirmed = await DisplayAlert("Delete income", "Are you sure you want to delete this income?", "Yes", "No");
                    if (userHasConfirmed) {
                        await viewModel.Remove(userIncome);
                    }
                });

            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {

        }
    }
}
