using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinalApp.ViewModels {

	public class MonthlyReport : BindableObject {
        private ReportsPageViewModel viewModel;

        public string MonthName { get; set; }
        public int MonthNumber { get; set; }

        public MonthlyReport(ReportsPageViewModel viewModel) {
            this.viewModel = viewModel;
        }

        public void OpenButtonClicked() { 
            if (viewModel is ReportsPageViewModel vm) {
                vm.OpenButtonClickedCommand.Execute(this);
            }
        }
    }

    public class ReportsPageViewModel : BindableObject {

    	protected readonly BindableProperty MonthlyReportsProperty =
            BindableProperty.Create(nameof(MonthlyReports), typeof(IEnumerable<MonthlyReport>), typeof(ReportsPageViewModel), new List<MonthlyReport>());

        protected readonly BindableProperty OpenButtonClickedCommandProperty =
            BindableProperty.Create(nameof(OpenButtonClickedCommand), typeof(Command<MonthlyReport>), typeof(ReportsPageViewModel), new Command<MonthlyReport>((obj)=> { }));

        public IEnumerable<MonthlyReport> MonthlyReports {
            get => (IEnumerable<MonthlyReport>)GetValue(MonthlyReportsProperty);
            set => SetValue(MonthlyReportsProperty, value);
        }

        public Command<MonthlyReport> OpenButtonClickedCommand {
            get => (Command<MonthlyReport>)GetValue(OpenButtonClickedCommandProperty);
            set => SetValue(OpenButtonClickedCommandProperty, value);
        }

        public ReportsPageViewModel() {
            MonthlyReports = new List<MonthlyReport> {
                new MonthlyReport(this) { MonthName = "January", MonthNumber = 1 },
                new MonthlyReport(this) { MonthName = "February", MonthNumber = 2 },
                new MonthlyReport(this) { MonthName = "March", MonthNumber = 3 },
                new MonthlyReport(this) { MonthName = "April", MonthNumber = 4 },
                new MonthlyReport(this) { MonthName = "May", MonthNumber = 5 },
                new MonthlyReport(this) { MonthName = "June", MonthNumber = 6 },
                new MonthlyReport(this) { MonthName = "July", MonthNumber = 7 },
                new MonthlyReport(this) { MonthName = "August", MonthNumber = 8 },
                new MonthlyReport(this) { MonthName = "September", MonthNumber = 9 },
                new MonthlyReport(this) { MonthName = "October", MonthNumber = 10 },
                new MonthlyReport(this) { MonthName = "November", MonthNumber = 11 },
                new MonthlyReport(this) { MonthName = "December", MonthNumber = 12 }
            };

            // UNCOMMENT TO FILTER BASED ON CURRENT MONTH
             int currentMonth = DateTime.Now.Month;
            // MonthlyReports = MonthlyReports.Where((m) => m.MonthNumber < currentMonth);
        }
    }
}
