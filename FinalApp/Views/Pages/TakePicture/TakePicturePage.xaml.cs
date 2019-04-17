using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FinalApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.TakePicture {
    public partial class TakePicturePage : ContentPage {
        public TakePicturePage() {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e) {
            MediaFile imgFile = await GoToCameraPicker();
            if (imgFile == null) {
                return;
            }

            ImageSource imgSource = ImageSource.FromStream(() => {
                var stream = imgFile.GetStream();
                imgFile.Dispose();
                return stream;
            });
            if (imgSource == null) {
                return;
            }

            using(var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<AnalyzePicturePageViewModel>() is AnalyzePicturePageViewModel viewModel) {
                    viewModel.UserImageFilePath = imgFile.Path;
                    viewModel.UserImageSource = imgSource;  
                    await Navigation.PushAsync(new AnalyzePicture.AnalyzePicturePage { BindingContext = viewModel }, true);
                }
            }
        }

        private async Task<MediaFile> GoToCameraPicker() {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return null;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {
                PhotoSize = PhotoSize.Medium,
                Directory = "Pictures",
                Name = "current.jpg"
            });

            return file;
        }
    }
}
