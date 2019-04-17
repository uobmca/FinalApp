using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.ViewModels;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.AnalyzePicture {
    public partial class AnalyzePicturePage : ContentPage {

        const string subscriptionKey = "f61a5349f7944216b220f05fc119e8a2";
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
            bool success = await MakeOCRRequest(imageFilePath);
            if (!success) {
                Device.BeginInvokeOnMainThread(async () => {
                    magnifyImage.IsVisible = false;
                    await DisplayAlert("Oops", "Something went wrong while analyzing your image. Please, give it another try", "Ok");
                    await Navigation.PopAsync();
                });
            }
        }

        static async Task<bool> MakeOCRRequest(string imageFilePath) {
            try {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "language=unk&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData)) {
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }

                if (response == null || !response.IsSuccessStatusCode || response.Content == null) {
                    if (response != null) {
                        Debug.Print("\nERROR Code: {0}", response.StatusCode);
                        string msg = await response.Content.ReadAsStringAsync();
                        Debug.Print("\nMessage: {0}", JToken.Parse(msg).ToString());
                    }
                    return false;
                }

                string contentString = await response.Content.ReadAsStringAsync();

                if (contentString == null) {
                    return false;
                }

                OcrApiResponse ocrResponse = OcrApiResponse.FromJson(contentString);

                Debug.Print("\nResponse:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());

                return true;
            } catch (Exception e) {
                Debug.Print("\n" + e.Message);
                return false;
            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath) {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read)) {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        private void RunAnimations() {
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
