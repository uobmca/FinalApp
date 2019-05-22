using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FinalApp.ViewModels;
using FinalApp.Views.Popups;
using Rg.Plugins.Popup.Services;
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
            SetupUI();
        }

        private void SetupUI() {
            ToolbarItems.Add(new ToolbarItem("Export PDF", "ic_save", async () => {
                if (BindingContext is ReportDetailPageViewModel viewModel) {
                    var popup = new ActivityIndicatorPopup();
                    await PopupNavigation.Instance.PushAsync(popup);
                    await viewModel.ExportPDF();
                    await PopupNavigation.Instance.RemovePageAsync(popup);
                }
            }));
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
