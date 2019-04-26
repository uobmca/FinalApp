using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class ExpensesListPageViewModel : BindableObject {

        protected readonly BindableProperty UserExpensesProperty =
            BindableProperty.Create(nameof(UserExpenses), typeof(IEnumerable<UserExpense>), typeof(ExpensesListPageViewModel), new List<UserExpense>());

        protected readonly BindableProperty CategoryIdProperty =
            BindableProperty.Create(nameof(CategoryId), typeof(string), typeof(ExpensesListPageViewModel), "");

        public IEnumerable<UserExpense> UserExpenses {
            get => (IEnumerable<UserExpense>)GetValue(UserExpensesProperty);
            set => SetValue(UserExpensesProperty, value);
        }

        public string CategoryId {
            get => (string)GetValue(CategoryIdProperty);
            set => SetValue(CategoryIdProperty, value);
        }

        private IUserDataRepository repository;

        public ExpensesListPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
            Update();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == CategoryIdProperty.PropertyName) {
                Update(CategoryId);
            }
        }

        public async Task Update(string categoryId) {
            UserExpenses = (await repository.GetUserExpenses()).Where((arg)=>arg.CategoryId == categoryId);
        }

        public async Task Update() {
            UserExpenses = await repository.GetUserExpenses();
        }
    }
}
