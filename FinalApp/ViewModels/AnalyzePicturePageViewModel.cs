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

        public int ExpenseType { get; set; }

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

                if (response == null) {
                    return null;
                }

                switch(ExpenseType) {
                    case SelectableUserExpense.ReceiptTypes.GenericReceipt:
                        ExtractedExpense = await dataExtractor.ExtractExpensesFromReceipt(response.RecognitionResult, ImageMetadata);
                        break;
                    case SelectableUserExpense.BillTypes.Type01:
                        ExtractedExpense = await dataExtractor.ExtractExpensesFromTypeOneBill(response.RecognitionResult, ImageMetadata);
                        break;
                    case SelectableUserExpense.BillTypes.Type02:
                        ExtractedExpense = await dataExtractor.ExtractExpensesFromTypeTwoBill(response.RecognitionResult, ImageMetadata);
                        break;
                    default:
                        ExtractedExpense = null;
                        break;
                }

                return response;
            }

            return null;
        }
    }
}
