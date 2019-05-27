using System;
using System.Collections.Generic;
using FinalApp.Models;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class ExpenseDetailRowTemplate : ViewCell {

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(ExpenseDetailRowTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public readonly static BindableProperty ExpenseDeleteCommandProperty =
            BindableProperty.Create(nameof(ExpenseDeleteCommand), typeof(Command<UserExpense>), typeof(ExpenseDetailRowTemplate), null);

        public Command<UserExpense> ExpenseDeleteCommand {
            get => (Command<UserExpense>) GetValue(ExpenseDeleteCommandProperty);
            set => SetValue(ExpenseDeleteCommandProperty, value);
        }

        public ExpenseDetailRowTemplate() {
            InitializeComponent();
            SetupListeners();
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
            if (newValue != oldValue && newValue != null) {
                (bindable as ExpenseDetailRowTemplate).ParentContext = newValue;
            }
        }

        private void SetupListeners() {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (e, sender) => {
                if (BindingContext is UserExpense expense) {
                    if (ParentContext is ExpensesListPageViewModel viewModel) {
                        if (viewModel.DeleteCommand is Command<UserExpense> deleteCommand) { 
                            deleteCommand.Execute(expense);
                        }
                    }
                }
            };
            deleteImage.GestureRecognizers.Add(tapGestureRecognizer);
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
