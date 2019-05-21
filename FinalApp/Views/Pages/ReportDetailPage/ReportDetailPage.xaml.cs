using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FinalApp.ViewModels;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.ReportDetailPage {
    public partial class ReportDetailPage : ContentPage {

        public ReportDetailPage() {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<ReportDetailPageViewModel>() is ReportDetailPageViewModel viewModel) {
                    BindingContext = viewModel;
                }
            }
        }

        protected async override void OnAppearing() {
            base.OnAppearing();
            await InitWebView();
        }

        private async Task InitWebView() {
            if (BindingContext is ReportDetailPageViewModel viewModel) {
                await viewModel.BuildHtmlContent();
                mainWebView.Source = new HtmlWebViewSource {
                    Html = viewModel.HtmlContent
                };
            }
        }
    }
}
