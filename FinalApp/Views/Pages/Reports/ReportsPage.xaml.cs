using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.Reports {
    public partial class ReportsPage : ContentPage {
        public ReportsPage() {
            InitializeComponent();
            using(var scope = App.Container.BeginLifetimeScope()) { 
                if (scope.Resolve<ReportsPageViewModel>() is ReportsPageViewModel viewModel) {
                    BindingContext = viewModel;
                    viewModel.OpenButtonClickedCommand = new Command<MonthlyReport>((MonthlyReport obj) => {
                    
                        var startDate = new DateTime(DateTime.Now.Year, obj.MonthNumber, 1);
                        var endDate = new DateTime(DateTime.Now.Year, obj.MonthNumber, DateTime.DaysInMonth(DateTime.Now.Year, obj.MonthNumber));

                        using (var innerScope = App.Container.BeginLifetimeScope()) { 
                            if(innerScope.Resolve<ReportDetailPageViewModel>() is ReportDetailPageViewModel vm) {
                                vm.StartDate = startDate;
                                vm.EndDate = endDate;
                                vm.IsMonthlyReport = true;

                                ReportDetailPage.ReportDetailPage page = new ReportDetailPage.ReportDetailPage {
                                    BindingContext = vm
                                };

                                Navigation.PushAsync(page);
                            }
                        }
                    });
                }
            }
        }

        void OnGetReportClicked(object sender, System.EventArgs e) {
            using (var scope = App.Container.BeginLifetimeScope()) { 
                if (scope.Resolve<ReportDetailPageViewModel>() is ReportDetailPageViewModel vm) {
                    vm.StartDate = new DateTime(startDatePicker.Date.Year, startDatePicker.Date.Month, startDatePicker.Date.Day, 0, 0, 0);
                    vm.EndDate = new DateTime(endDatePicker.Date.Year, endDatePicker.Date.Month, endDatePicker.Date.Day, 23, 59, 59);
                    vm.IsMonthlyReport = false;

                    ReportDetailPage.ReportDetailPage page = new ReportDetailPage.ReportDetailPage {
                        BindingContext = vm
                    };

                    Navigation.PushAsync(page);
                }
            }
        }
    }
}
