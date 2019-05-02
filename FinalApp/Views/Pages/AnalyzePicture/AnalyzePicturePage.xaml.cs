using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using FinalApp.Models;
using FinalApp.ViewModels;
using FinalApp.Views.Base;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.AnalyzePicture {
    public partial class AnalyzePicturePage : ContentPage {

        const string subscriptionKey = "d52564bdb77443deacdb5dba486a471a";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr";
        public ImageSource UserImageSource { get; set; } = null;
        private bool shouldAnimate = true;

        public AnalyzePicturePage() {
            InitializeComponent();
            RunAnimations();
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext is AnalyzePicturePageViewModel viewModel) {
                StartAnalyzing(viewModel.UserImageFilePath);
            }
        }

        private async void StartAnalyzing(string imageFilePath) {

            if (BindingContext is AnalyzePicturePageViewModel viewModel) {
                var response = await viewModel.MakeOCRRequest(imageFilePath);
                if (response == null || response.Status != "Succeeded" || viewModel.ExtractedExpense == null) {
                    Device.BeginInvokeOnMainThread(async () => {
                        magnifyImage.IsVisible = false;
                        await DisplayAlert("Oops", "Something went wrong while analyzing your image. Please, give it another try", "Ok");
                        await Navigation.PopAsync();
                    });
                } else {
                    var page = new ExpenseDetail.ExpenseDetailPage();

                    using (var scope = App.Container.BeginLifetimeScope()) { 
                        if (scope.Resolve<ExpenseDetailPageViewModel>() is ExpenseDetailPageViewModel vm) {
                            vm.SelectedUserExpense = viewModel.ExtractedExpense;
                            page.BindingContext = vm;
                            await Navigation.PushModalAsync(new AppNavigationPage(page));
                            await Navigation.PopToRootAsync();
                        }
                    }


                }
            }
        }

        private void RunAnimations() {
            if(Device.RuntimePlatform == Device.iOS) { 
                Device.BeginInvokeOnMainThread(async () => {
                    while (shouldAnimate) {
                        await magnifyImage.TranslateTo(80, -80, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(50, -80, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(90, 30, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(100, 80, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(20, 30, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(-100, 30, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(-50, 80, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(-30, 30, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(100, 80, 1000, Easing.SpringIn);
                        await magnifyImage.TranslateTo(-80, 45, 1000, Easing.SpringIn);
                    }
                });
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            magnifyImage.IsVisible = true;
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            shouldAnimate = false;
        }
    }
}
