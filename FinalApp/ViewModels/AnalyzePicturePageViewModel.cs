using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinalApp.Commons;
using FinalApp.Models;
using FinalApp.Network;
using Polly;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class AnalyzePicturePageViewModel : BindableObject {

        const int kResultRequestMaxRetryAttempts = 8;
        TimeSpan kResultRequestsInterval = TimeSpan.FromSeconds(3);

        protected static readonly BindableProperty UserImageSourceProperty =
            BindableProperty.Create(nameof(UserImageSource), typeof(ImageSource), typeof(AnalyzePicturePageViewModel), null);

        protected static readonly BindableProperty UserImageFilePathProperty =
            BindableProperty.Create(nameof(UserImageFilePath), typeof(string), typeof(AnalyzePicturePageViewModel), null);

        public ImageSource UserImageSource {
            get => (ImageSource)GetValue(UserImageSourceProperty);
            set => SetValue(UserImageSourceProperty, value);
        }

        public string UserImageFilePath {
            get => (string)GetValue(UserImageFilePathProperty);
            set => SetValue(UserImageFilePathProperty, value);
        }

        private INetworkOCRServices ocrServices;

        public AnalyzePicturePageViewModel(INetworkOCRServices ocrServices) {
            this.ocrServices = ocrServices;
        }

        public async Task<CognitiveServicesResponse> MakeOCRRequest(string imageFilePath, bool isHandWritten = true) {
            byte[] byteData = ImageUtils.GetImageAsByteArray(imageFilePath);
            string result = await ocrServices.SendCognitiveServicesRequest(byteData, isHandWritten);
            if (result is string operationId) {

                 CognitiveServicesResponse response = await Policy
                    .Handle<HttpRequestException>()
                    .OrResult<CognitiveServicesResponse>(r => r.Status != "Succeeded")
                    .WaitAndRetryAsync(kResultRequestMaxRetryAttempts, (i) => kResultRequestsInterval)
                    .ExecuteAsync(()=>ocrServices.GetCognitiveServicesResponse(operationId));

                return response;
            }

            return null;
        }
    }
}
