using System;
using Xamarin.Forms;
using FinalApp.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace FinalApp.ViewModels {

    public class SelectExpensePageViewModel : BindableObject {

        protected readonly BindableProperty IsItemsSourceEmptyProperty =
            BindableProperty.Create(nameof(IsItemsSourceEmpty), typeof(bool), typeof(SelectExpensePageViewModel), true);

        protected readonly BindableProperty IsReceiptProperty =
    		BindableProperty.Create(nameof(IsReceipt), typeof(bool), typeof(SelectExpensePageViewModel), true);

        protected readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<SelectableUserExpense>), typeof(SelectExpensePageViewModel), new List<SelectableUserExpense>());

        protected readonly BindableProperty ItemsSourceCountProperty =
            BindableProperty.Create(nameof(ItemsSourceCount), typeof(int), typeof(SelectExpensePageViewModel), 0);

        public bool IsItemsSourceEmpty {
            get => (bool)GetValue(IsItemsSourceEmptyProperty);
            set => SetValue(IsItemsSourceEmptyProperty, value);
        }

        public bool IsReceipt {
			get => (bool) GetValue(IsReceiptProperty);
			set => SetValue(IsReceiptProperty, value);
		}

        public IEnumerable<SelectableUserExpense> ItemsSource {
            get => (IEnumerable<SelectableUserExpense>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int ItemsSourceCount {
            get => (int)GetValue(ItemsSourceCountProperty);
            set => SetValue(ItemsSourceCountProperty, value);
        }

        private SelectableUserExpense[] selectableBillTemplates = new SelectableUserExpense[] {
            new SelectableUserExpense { ExpenseImage = "bill_type_01", DisplayName = "Type one", ExpenseType = SelectableUserExpense.BillTypes.Type01 },
            new SelectableUserExpense { ExpenseImage = "bill_type_02", DisplayName = "Type two", ExpenseType = SelectableUserExpense.BillTypes.Type02 },
        };

        private SelectableUserExpense[] selectableReceiptTemplates = new SelectableUserExpense[] {
            new SelectableUserExpense { ExpenseImage = "receipts_generic", DisplayName = "Generic Receipt", ExpenseType = SelectableUserExpense.ReceiptTypes.GenericReceipt }
        };

        public SelectExpensePageViewModel() {
            UpdateItemsSource();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if(propertyName == IsReceiptProperty.PropertyName) {
                UpdateItemsSource();
            }
        }

        private async Task UpdateItemsSource() { 
            ItemsSource = null;
            ItemsSource = IsReceipt ? selectableReceiptTemplates : selectableBillTemplates;
            ItemsSourceCount = ItemsSource.Count();
            IsItemsSourceEmpty = ItemsSourceCount == 0;
        } 
    }
}
