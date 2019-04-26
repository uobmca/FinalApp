using System;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class IncomeDetailPageViewModel : BindableObject {

        protected readonly BindableProperty SelectedUserIncomeProperty =
            BindableProperty.Create(nameof(SelectedUserIncome), typeof(UserIncome), typeof(ExpenseDetailPageViewModel), new UserIncome());

        public UserIncome SelectedUserIncome {
            get => (UserIncome)GetValue(SelectedUserIncomeProperty);
            set => SetValue(SelectedUserIncomeProperty, value);
        }

        private IUserDataRepository repository;

        public IncomeDetailPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }

        public async Task Save() {
            if (SelectedUserIncome != null) { 
                await repository.SaveUserIncome(SelectedUserIncome);
            }
        }
    }
}

