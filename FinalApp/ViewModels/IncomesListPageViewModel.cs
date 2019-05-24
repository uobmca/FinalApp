using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FinalApp.ViewModels {
    public class IncomesListPageViewModel : BindableObject {

        protected readonly BindableProperty UserIncomesProperty =
            BindableProperty.Create(nameof(UserIncomes), typeof(IEnumerable<UserIncome>), typeof(IncomesListPageViewModel), new List<UserIncome>());

        protected readonly BindableProperty CategoryIdProperty =
            BindableProperty.Create(nameof(CategoryId), typeof(string), typeof(IncomesListPageViewModel), "");

        public IEnumerable<UserIncome> UserIncomes {
            get => (IEnumerable<UserIncome>)GetValue(UserIncomesProperty);
            set => SetValue(UserIncomesProperty, value);
        }

        public string CategoryId {
            get => (string)GetValue(CategoryIdProperty);
            set => SetValue(CategoryIdProperty, value);
        }

        private IEnumerable<Category> userCategories;
        private readonly IUserDataRepository repository;

        public IncomesListPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == CategoryIdProperty.PropertyName) {
                Update(CategoryId);
            }
        }

        public async Task Update() {
            UserIncomes = await repository.GetUserIncomes();
        }

        public async Task Update(string categoryId) {
            userCategories = await repository.GetUserCategories();
            UserIncomes = (await repository.GetUserIncomes()).Where((inc) => inc.CategoryId == categoryId);
            foreach (UserIncome income in UserIncomes) {
                income.UserCategory = userCategories.FirstOrDefault((category) => category.Id == income.CategoryId);
            }
        }
    }
}
