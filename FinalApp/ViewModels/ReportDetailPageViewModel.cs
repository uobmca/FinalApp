using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinalApp.Commons;
using FinalApp.Models;
using FinalApp.Network;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class ReportDetailPageViewModel : BindableObject {

        private const string monthNamePlaceholder = "{MonthName}";
        private const string incomesTotalPlaceholder = "{IncomeValue}";
        private const string expensesTotalPlaceholder = "{ExpensesValue}";
        private const string balanceValuePlacholder = "{BalanceValue}";
        private const string incomesInnerFieldsPlaceholder = "{Income_InnerFields}";
        private const string expensesInnerFieldsPlaceholder = "{Expenses_InnerFields}";
        private const string newEntryStringFormat = "\t  <tr>\n\t    <td class=\"inner-field\">{0}</td>\n\t    <td >{1:C}</td>\n\t  </tr>";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsMonthlyReport { get; set; }

        public string HtmlContent = "<!DOCTYPE html>\n<html>\n<head>\n<meta name=\"viewport\" " +
        	"content=\"width=device-width, initial-scale=0.9\">\n<style>\n\nbody {\n\twidth:100%;\n}\n\ntable, th, " +
        	"td {\n  padding: 5px;\n  font-size: 12pt;\n}\n\n.income-value {\n\tcolor:green;\n    " +
        	"font-size:14pt;\n}\n\n.expenses-value {\n\tcolor:red;\n    " +
        	"font-size:14pt;\n}\n\n.balance-value {\n\tcolor:blue;\n    " +
        	"font-size:14pt;\n}\n\n.inner-field {\n\tdisplay:inline-block;\n\tmargin-left:2em;\n}\n\n.header-title {\n\tmargin-left: auto;\n\tmargin-right:auto;\n\ttext-align: center;\n   " +
        	"\tdisplay: block;\n}\n\n.center-content {\n\ttext-align: center;\n\tmargin-left:auto;\n\tmargin-right:auto;\n\tdisplay: block;\n}\n\ntable {\n  border-spacing: 15px;\n}\n</style>\n" +
        	"</head>\n<body>\n<div class=\"center-content\">\n\t<div class=\"header-title\">\n\t\t<h2>{MonthName} Report</h2>\n\t\t<p>Income / " +
        	"Expense Report</p>\n\t</div>\n\n\n\t<table style=\"width:100%\">\n\t  <tr>\n\t    <td><b>Income</b></td>\n\t    " +
        	"<td class=\"income-value\">{IncomeValue}</td>\n\t  </tr>\n\t  \n\t  {Income_InnerFields}\n\n\t  <tr>\n\t    " +
        	"<td><b>Expenses</b></td>\n\t    <td class=\"expenses-value\">{ExpensesValue}</td>\n\t  </tr>\n\n\t  " +
        	"{Expenses_InnerFields}\n\n\t  <tr>\n\t    <td><b>Balance</b></td>\n\t    " +
        	"<td class=\"balance-value\">{BalanceValue}</td>\n\t  </tr>\n\n\n\t  " +
        	"\n\t</table>\n</div>\n</body>\n</html>\n";

        private IUserDataRepository repository;
        private DocumentsUtilsApiService documentsApiService;

        public ReportDetailPageViewModel(IUserDataRepository repository, DocumentsUtilsApiService documentsApiService) {
            this.repository = repository;
            this.documentsApiService = documentsApiService;
        }

        public async Task BuildHtmlContent() {
        
            List<Category> categories = await repository.GetUserCategories();
            List<UserIncome> incomes = await repository.GetUserIncomes(StartDate,EndDate);
            List<UserExpense> expenses = await repository.GetUserExpenses(StartDate, EndDate);

            string outContent = HtmlContent;

            // Sets up title content
            string monthNameContent = IsMonthlyReport ?
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartDate.Month).Capitalize()
                : string.Format("From {0} to {1}", StartDate.ToShortDateString(), EndDate.ToShortDateString());

            // Sets up incomes and expenses content
            double totalIncomes = incomes.Sum((income) => income.Amount);
            double totalExpenses = expenses.Sum((expense) => expense.Amount);
            double balance = totalIncomes - totalExpenses;

            // Fill income inner fields
            string incomeInnerFields = BuildIncomeInnerFields(incomes, categories);
            string expensesInnerFields = BuildExpensesInnerFields(expenses, categories);

            // Replaces placeholders with new content
            outContent = outContent.Replace(monthNamePlaceholder, monthNameContent);
            outContent = outContent.Replace(incomesTotalPlaceholder, string.Format("{0:C}", totalIncomes));
            outContent = outContent.Replace(expensesTotalPlaceholder, string.Format("{0:C}", totalExpenses));
            outContent = outContent.Replace(balanceValuePlacholder, string.Format("{0:C}", balance));
            outContent = outContent.Replace(incomesInnerFieldsPlaceholder, incomeInnerFields);
            outContent = outContent.Replace(expensesInnerFieldsPlaceholder, expensesInnerFields);

            HtmlContent = outContent;
        }

        private string BuildIncomeInnerFields(IEnumerable<UserIncome> incomes, IEnumerable<Category> categories) {
            string outString = "";
            var groupedIncomes = incomes.GroupBy((income) => income.CategoryId);

            foreach(IGrouping<string, UserIncome> item in groupedIncomes) {
                if (categories.FirstOrDefault((arg) => arg.Id.Equals(item.Key)) is Category category) {
                    var categoryName = category.DisplayName;
                    var categoryTotal = item.Sum((UserIncome arg) => arg.Amount);
                    outString += string.Format(newEntryStringFormat, categoryName, categoryTotal);
                }
            }

            return outString;
        }

        private string BuildExpensesInnerFields(IEnumerable<UserExpense> expenses, IEnumerable<Category> categories) {
            string outString = "";
            var groupedExpenses = expenses.GroupBy((expense) => expense.CategoryId);

            foreach (IGrouping<string, UserExpense> item in groupedExpenses) {
                if (categories.FirstOrDefault((arg) => arg.Id.Equals(item.Key)) is Category category) {
                    var categoryName = category.DisplayName;
                    var categoryTotal = item.Sum((UserExpense arg) => arg.Amount);
                    outString += string.Format(newEntryStringFormat, categoryName, categoryTotal);
                }
            }

            return outString;
        }

        public async Task ExportPDF() {
            var response = await documentsApiService.ConvertHtmlToPdf(HtmlContent);
            if(response.IsSuccess) {
                var fileName = string.Format("report_{0}_{1}.pdf", StartDate.ToFileTimeUtc(), EndDate.ToFileTimeUtc());
                var fileService = DependencyService.Get<IFileService>();
                using (var stream = new MemoryStream(response.FileBytesData)) {
                    fileService.SaveDataStream(fileName, stream, "Reports");
                    fileService.ShowDocument(fileName);
                }
            }
        }

    }
}
