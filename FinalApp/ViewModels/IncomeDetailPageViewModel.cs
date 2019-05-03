using System;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace FinalApp.ViewModels {
    public class IncomeDetailPageViewModel : BindableObject {

        protected readonly BindableProperty SelectedUserIncomeProperty =
            BindableProperty.Create(nameof(SelectedUserIncome), typeof(UserIncome), typeof(IncomeDetailPageViewModel), new UserIncome {
                IncomeDate = DateTime.Now
            });

        protected readonly BindableProperty SelectedUserCategoryProperty =  
            BindableProperty.Create(nameof(SelectedUserCategory), typeof(Category), typeof(IncomeDetailPageViewModel), new Category());

        protected readonly BindableProperty UserCategoriesProperty =
            BindableProperty.Create(nameof(UserCategories), typeof(IEnumerable<Category>), typeof(IncomeDetailPageViewModel), new List<Category>());

        public Category SelectedUserCategory {
            get => (Category)GetValue(SelectedUserCategoryProperty);
            set => SetValue(SelectedUserCategoryProperty, value);
        }

        public IEnumerable<Category> UserCategories {
            get => (IEnumerable<Category>)GetValue(UserCategoriesProperty);
            set => SetValue(UserCategoriesProperty, value);
        }

        public UserIncome SelectedUserIncome {
            get => (UserIncome)GetValue(SelectedUserIncomeProperty);
            set => SetValue(SelectedUserIncomeProperty, value);
        }

        private IUserDataRepository repository;

        public IncomeDetailPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }

        public async Task Update() {
            UserCategories = await repository.GetUserCategories();

            if(SelectedUserCategory == null || !UserCategories.Contains(SelectedUserCategory)) {
                SelectedUserCategory = UserCategories.FirstOrDefault();
            }
        }

        public async Task Save() {
            if (SelectedUserCategory != null) {
                SelectedUserIncome.CategoryId = SelectedUserCategory.Id;
            }
            if (SelectedUserIncome != null) { 
                await repository.SaveUserIncome(SelectedUserIncome);
            }
        }
    }
}

