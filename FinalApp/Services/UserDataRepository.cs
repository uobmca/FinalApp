using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using FinalApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace FinalApp.Services {
    public class UserDataRepository : IUserDataRepository {

        const string kApplicationUrl = "https://xamarinfinalapp.azurewebsites.net";
        const string kOfflineDatabasePath = "db_local_finalapp.db";

        MobileServiceClient mobileClient;
        IMobileServiceSyncTable<UserExpense> expensesTable;
        IMobileServiceSyncTable<UserIncome> incomesTable;
        IMobileServiceSyncTable<Category> categoriesTable;

        public UserDataRepository() {

            //mobileClient = new MobileServiceClient(kApplicationUrl);

            //var store = new MobileServiceSQLiteStore(kOfflineDatabasePath);
            //store.DefineTable<UserExpense>();
            ////store.DefineTable<UserIncome>();
            ////store.DefineTable<Category>();

            //mobileClient.SyncContext.InitializeAsync(store);

            //expensesTable = mobileClient.GetSyncTable<UserExpense>();
            //incomesTable = mobileClient.GetSyncTable<UserIncome>();
            //categoriesTable = mobileClient.GetSyncTable<Category>();
        }

        private async Task SyncAsync() {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try {
                await mobileClient.SyncContext.PushAsync();

                await expensesTable.PullAsync(
                    nameof(expensesTable),
                    expensesTable.CreateQuery());

                //await incomesTable.PullAsync(
                //    nameof(incomesTable),
                //    incomesTable.CreateQuery());

                //await categoriesTable.PullAsync(
                    //nameof(categoriesTable),
                    //categoriesTable.CreateQuery());
            } catch (MobileServicePushFailedException exc) {
                if (exc.PushResult != null) {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null) {
                foreach (var error in syncErrors) {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null) {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    } else {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }

        public async Task<List<UserExpense>> GetUserExpenses() {
            try {
                await SyncAsync();
                return await expensesTable.ToListAsync();
            } catch (MobileServiceInvalidOperationException msioe) {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            } catch (Exception e) {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveUserExpense(UserExpense expense) {
            if (expense.CategoryId == -1) {
                await expensesTable.InsertAsync(expense);
            } else {
                await expensesTable.UpdateAsync(expense);
            }
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



        public async Task SaveUserExpenses(IEnumerable<UserExpense> incomes) {
            throw new NotImplementedException();
        }

        public async Task SaveUserIncome(UserIncome income) {
            throw new NotImplementedException();
        }

        public async Task SaveUserIncomes(IEnumerable<UserIncome> incomes) {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetUserCategories() {
            return new List<Category>() {
                new Category {
                    DisplayName = "House",
                    CategoryId = 1,
                    Icon = "ic_camera"
                },
                new Category {
                    DisplayName = "Car",
                    CategoryId = 2,
                    Icon = "ic_camera"
                },
                new Category {
                    DisplayName = "Entertainment",
                    CategoryId = 3,
                    Icon = "ic_camera"
                },
            };
        }

        public Task SaveUserCategory(Category category) {
            throw new NotImplementedException();
        }
    }
}
