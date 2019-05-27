using System;
using System.Collections.Generic;
using FinalApp.Models;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class IncomeDetailRowTemplate : ViewCell {

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(IncomeDetailRowTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public readonly static BindableProperty IncomeDeleteCommandProperty =
            BindableProperty.Create(nameof(IncomeDeleteCommand), typeof(Command<UserIncome>), typeof(IncomeDetailRowTemplate), null);

        public Command<UserIncome> IncomeDeleteCommand {
            get => (Command<UserIncome>) GetValue(IncomeDeleteCommandProperty);
            set => SetValue(IncomeDeleteCommandProperty, value);
        }

        public IncomeDetailRowTemplate() {
            InitializeComponent();
            SetBinding(IncomeDeleteCommandProperty, new Binding("ParentContext.DeleteCommand", BindingMode.Default, null, null, null, this));
            SetupListeners();
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
            if (newValue != oldValue && newValue != null) {
                (bindable as IncomeDetailRowTemplate).ParentContext = newValue;
            }
        }

        private void SetupListeners() {

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (e, sender) => {
                if (BindingContext is UserIncome income) {
                    if (ParentContext is IncomesListPageViewModel viewModel) {
                        if (viewModel.DeleteCommand is Command<UserIncome> deleteCommand) { 
                            deleteCommand.Execute(income);
                        }
                    }
                }
            };
            deleteImage.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is UserIncome income) {
                dateLabel.Text = income.IncomeDate.Date.ToShortDateString();
                amountLabel.Text = string.Format("{0:C}", income.Amount);
                //categoryIconImage.Source = income.UserCategory != null ? (income.UserCategory.Icon != "" ? income.UserCategory.Icon : "ic_tag_home") : "ic_tag_home";
            }
        }
    }
}
