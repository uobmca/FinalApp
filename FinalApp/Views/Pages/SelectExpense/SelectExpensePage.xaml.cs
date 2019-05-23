using System;
using System.Collections.Generic;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.SelectExpense {
    public partial class SelectExpensePage : ContentPage {

        private enum ItemsSourceType { 
            Receipt,
            Bill
        }

        public SelectExpensePage() {
            InitializeComponent();
            SetupBindingContext();
            SetupListeners();
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
    }
}
