﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using FinalApp.Commons;
using FinalApp.Models;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.SelectExpense {
    public partial class SelectExpensePage : ContentPage {

        public Command<SelectableUserExpense> OnUserExpenseTypeSelected { get; set; }

        private enum ItemsSourceType { 
            Receipt,
            Bill
        }

        public SelectExpensePage() {
            InitializeComponent();
            SetupBindingContext();
            SetupListeners();
        }

        public static async Task PromptUserForExpenseType(INavigation navigation, Action<SelectableUserExpense> completion) {
            var page = new SelectExpensePage { OnUserExpenseTypeSelected = new Command<SelectableUserExpense>(completion) };
            await navigation.PushModalAsyncUnique(page);
        }

        private void SetupBindingContext() {
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<SelectExpensePageViewModel>() is SelectExpensePageViewModel viewModel) {
                    BindingContext = viewModel;
                }
            }
        }

        private void SetupListeners() {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) => { 
                if (sender == receiptSelectableFrame) {
                    SetItemsSourceType(ItemsSourceType.Receipt);
                } else if (sender == billSelectableFrame) {
                    SetItemsSourceType(ItemsSourceType.Bill);
                }
            };
            billSelectableFrame.GestureRecognizers.Add(tapGestureRecognizer);
            receiptSelectableFrame.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private void SetItemsSourceType(ItemsSourceType type) {

            if (!(BindingContext is SelectExpensePageViewModel viewModel)) return;

            viewModel.IsReceipt = (type == ItemsSourceType.Receipt);

            switch (type) {
                case ItemsSourceType.Receipt:
                    billSelectableFrame.Opacity = 0.3;
                    receiptSelectableFrame.Opacity = 1.0;
                    break;
                case ItemsSourceType.Bill:
                    receiptSelectableFrame.Opacity = 0.3;
                    billSelectableFrame.Opacity = 1.0;
                    break;
            }
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            if(OnUserExpenseTypeSelected is Command<SelectableUserExpense> listener) {
                await Navigation.PopModalAsync();
                listener.Execute(e.SelectedItem);
            }
        }
    }
}
