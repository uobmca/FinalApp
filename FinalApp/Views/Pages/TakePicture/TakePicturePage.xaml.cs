using System;
using System.Collections.Generic;
using System.IO;
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

        public static byte[] ReadFully(Stream input) {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream()) {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e) {
            MediaFile imgFile = await GoToCameraPicker();
            if (imgFile == null) {
                return;
            }

            var stream = imgFile.GetStream();

            byte[] imageBytes = ReadFully(stream);

            ImageSource imgSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

            //imgFile.Dispose();

            if (imgSource == null) {
                return;
            }

            using(var scope = App.Container.BeginLifetimeScope()) {
                if (scope.Resolve<AnalyzePicturePageViewModel>() is AnalyzePicturePageViewModel viewModel) {
                    viewModel.UserImageFilePath = imgFile.Path;
                    viewModel.UserImageSource = imgSource;
                    imgFile.Dispose();
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
