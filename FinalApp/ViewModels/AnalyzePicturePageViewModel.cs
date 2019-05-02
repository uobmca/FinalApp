using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinalApp.Commons;
using FinalApp.Models;
using FinalApp.Network;
using FinalApp.Services;
using Polly;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class AnalyzePicturePageViewModel : BindableObject {

        const int kResultRequestMaxRetryAttempts = 8;
        TimeSpan kResultRequestsInterval = TimeSpan.FromSeconds(3);

        protected readonly BindableProperty UserImageSourceProperty =
            BindableProperty.Create(nameof(UserImageSource), typeof(ImageSource), typeof(AnalyzePicturePageViewModel), null);

        protected readonly BindableProperty UserImageFilePathProperty =
            BindableProperty.Create(nameof(UserImageFilePath), typeof(string), typeof(AnalyzePicturePageViewModel), null);

        public ImageSource UserImageSource {
            get => (ImageSource)GetValue(UserImageSourceProperty);
            set => SetValue(UserImageSourceProperty, value);
        }

        public string UserImageFilePath {
            get => (string)GetValue(UserImageFilePathProperty);
            set => SetValue(UserImageFilePathProperty, value);
        }
        public Metadata ImageMetadata { get; private set; }

        public UserExpense ExtractedExpense { get; private set; }

        private INetworkOCRServices ocrServices;
        private IOCRDataExtractor dataExtractor;

        public AnalyzePicturePageViewModel(INetworkOCRServices ocrServices, IOCRDataExtractor dataExtractor) {
            this.ocrServices = ocrServices;
            this.dataExtractor = dataExtractor;
        }

        public async Task ExtractData() { 

        }

        public async Task<CognitiveServicesResponse> MakeOCRRequest(string imageFilePath, bool isHandWritten = true) {
            byte[] byteData = ImageUtils.GetImageAsByteArray(imageFilePath);
            CognitiveServicesRequestResponse result = await ocrServices.SendCognitiveServicesRequest(byteData, isHandWritten);

            ImageMetadata = result.ImageMetadata;

            if (result.OperationId is string operationId) {

                 CognitiveServicesResponse response = await Policy
                    .Handle<HttpRequestException>()
                    .OrResult<CognitiveServicesResponse>(r => r.Status != "Succeeded")
                    .WaitAndRetryAsync(kResultRequestMaxRetryAttempts, (i) => kResultRequestsInterval)
                    .ExecuteAsync(()=>ocrServices.GetCognitiveServicesResponse(operationId));

                ExtractedExpense = await dataExtractor.ExtractExpensesFromReceipt(response.RecognitionResult, ImageMetadata);

                return response;
            }

            return null;
        }
    }
}
