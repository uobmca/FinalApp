using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Network;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class DashboardPageViewModel : BindableObject {

        protected readonly BindableProperty UserExpensesProperty =
            BindableProperty.Create(nameof(UserExpenses), typeof(IEnumerable<UserExpense>), typeof(DashboardPageViewModel), new List<UserExpense>());

        protected readonly BindableProperty UserIncomesProperty =
            BindableProperty.Create(nameof(UserIncomes), typeof(IEnumerable<UserIncome>), typeof(DashboardPageViewModel), new List<UserIncome>());

        protected readonly BindableProperty UserIncomesSumProperty =
            BindableProperty.Create(nameof(UserIncomesSum), typeof(string), typeof(DashboardPageViewModel), "-");

        protected readonly BindableProperty UserExpensesSumProperty =
            BindableProperty.Create(nameof(UserExpensesSum), typeof(string), typeof(DashboardPageViewModel), "-");

        protected readonly BindableProperty UserBalanceProperty =
            BindableProperty.Create(nameof(UserBalance), typeof(string), typeof(DashboardPageViewModel), "-");

        protected readonly BindableProperty UserGreetingProperty =
            BindableProperty.Create(nameof(UserGreeting), typeof(string), typeof(DashboardPageViewModel), "Hi, User!");

        public IEnumerable<UserIncome> UserIncomes {
            get => (IEnumerable<UserIncome>)GetValue(UserIncomesProperty);
            set => SetValue(UserIncomesProperty, value);
        }

        public IEnumerable<UserExpense> UserExpenses {
            get => (IEnumerable<UserExpense>)GetValue(UserExpensesProperty);
            set => SetValue(UserExpensesProperty, value);
        }

        public string UserIncomesSum {
            get => (string)GetValue(UserIncomesSumProperty);
            set => SetValue(UserIncomesSumProperty, value);
        }

        public string UserExpensesSum {
            get => (string)GetValue(UserExpensesSumProperty);
            set => SetValue(UserExpensesSumProperty, value);
        }

        public string UserBalance {
            get => (string)GetValue(UserBalanceProperty);
            set => SetValue(UserBalanceProperty, value);
        }

        public string UserGreeting {
            get => (string)GetValue(UserGreetingProperty);
            set => SetValue(UserGreetingProperty, value);
        }

        private IUserDataRepository repository;

        public DashboardPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }

        public async Task Update() {

            UserExpenses = await repository.GetUserExpenses();
            UserIncomes = await repository.GetUserIncomes();

            var incomesSumValue = UserIncomes.Sum((inc) => inc.Amount);
            var expensesSumValue = UserExpenses.Sum((exp) => exp.Amount);

            UserIncomesSum = string.Format("{0:C}", incomesSumValue);
            UserExpensesSum = string.Format("{0:C}", expensesSumValue);

            var balance = incomesSumValue - expensesSumValue;
            string signStr = (balance == 0) ? "" : (balance > 0 ? "+" : "-");

            UserBalance = string.Format("{0} {1:C}", signStr, Math.Abs(balance));

            if (LoginManager.Instance.UserName is string username) {
                if (username != "") {
                    UserGreeting = string.Format("Hi, {0}!", username);
                }
            };
        }

    }
}
