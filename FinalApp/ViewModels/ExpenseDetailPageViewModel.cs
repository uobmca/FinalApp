using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class ExpenseDetailPageViewModel : BindableObject {

        protected readonly BindableProperty SelectedUserExpenseProperty =
            BindableProperty.Create(nameof(SelectedUserExpense), typeof(UserExpense), typeof(ExpenseDetailPageViewModel), new UserExpense { 
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ExpireDate = DateTime.Now
            });

        protected readonly BindableProperty SelectedUserCategoryProperty =
            BindableProperty.Create(nameof(SelectedUserCategory), typeof(Category), typeof(ExpenseDetailPageViewModel), new Category());

        protected readonly BindableProperty UserCategoriesProperty =
            BindableProperty.Create(nameof(UserCategories), typeof(IEnumerable<Category>), typeof(ExpenseDetailPageViewModel), new List<Category>());

        public UserExpense SelectedUserExpense {
            get => (UserExpense)GetValue(SelectedUserExpenseProperty);
            set => SetValue(SelectedUserExpenseProperty, value);
        }

        public Category SelectedUserCategory {
            get => (Category)GetValue(SelectedUserCategoryProperty);
            set => SetValue(SelectedUserCategoryProperty, value);
        }

        public IEnumerable<Category> UserCategories {
            get => (IEnumerable<Category>)GetValue(UserCategoriesProperty);
            set => SetValue(UserCategoriesProperty, value);
        }

        private IUserDataRepository repository;

        public ExpenseDetailPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
            Update();
        }

        public async Task Update() {
            UserCategories = await repository.GetUserCategories();

            if(SelectedUserCategory == null || !UserCategories.Contains(SelectedUserCategory)) {
                SelectedUserCategory = UserCategories.First();
            }
        }

        public async Task Save() {
            if (SelectedUserCategory != null) {
                SelectedUserExpense.CategoryId = SelectedUserCategory.Id;
            } 
            await repository.SaveUserExpense(SelectedUserExpense);
        } 
    }
}

