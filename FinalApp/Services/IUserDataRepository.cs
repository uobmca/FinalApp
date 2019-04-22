using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinalApp.Models;

namespace FinalApp.Services {
    public interface IUserDataRepository {
        // Expenses
        Task<List<UserExpense>> GetUserExpenses();
        Task<List<UserExpense>> GetUserExpenses(DateTime startDate, DateTime endDate);

        Task SaveUserExpense(UserExpense income);
        Task SaveUserExpenses(IEnumerable<UserExpense> incomes);

        // Incomes
        Task<List<UserIncome>> GetUserIncomes();
        Task<List<UserIncome>> GetUserIncomes(DateTime startDate, DateTime endDate);

        Task SaveUserIncome(UserIncome income);
        Task SaveUserIncomes(IEnumerable<UserIncome> incomes);

        // Categories
        Task<List<Category>> GetUserCategories();
        Task SaveUserCategory(Category category);
    }
}
