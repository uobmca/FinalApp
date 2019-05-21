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
    }
}
