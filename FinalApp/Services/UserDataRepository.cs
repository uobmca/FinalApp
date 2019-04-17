using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinalApp.Models;

namespace FinalApp.Services {
    public class UserDataRepository : IUserDataRepository {

        public async Task<List<UserExpense>> GetUserExpenses() {
            throw new NotImplementedException();
        }

        public async Task<List<UserExpense>> GetUserExpenses(DateTime startDate, DateTime endDate) {
            throw new NotImplementedException();
        }

        public async Task<List<UserIncome>> GetUserIncomes() {
            throw new NotImplementedException();
        }

        public async Task<List<UserIncome>> GetUserIncomes(DateTime startDate, DateTime endDate) {
            throw new NotImplementedException();
        }

        public async Task SaveUserExpense(UserExpense income) {
            throw new NotImplementedException();
        }

        public async Task SaveUserExpenses(IEnumerable<UserExpense> incomes) {
            throw new NotImplementedException();
        }

        public async Task SaveUserIncome(UserIncome income) {
            throw new NotImplementedException();
        }

        public async Task SaveUserIncomes(IEnumerable<UserIncome> incomes) {
            throw new NotImplementedException();
        }

    }
}
