using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.TakePicture {
    public partial class TakePicturePage : ContentPage {
        public TakePicturePage() {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e) {
            ImageSource imgSource = await GoToCameraPicker();
        }

        private async Task<ImageSource> GoToCameraPicker() {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                Directory = "Pictures",
                Name = "current.jpg"
            });

            if (file == null) {
                return null;
            }

            await DisplayAlert("File Location", string.Format("Succesfully saved to {0}", file.Path), "OK");

            return ImageSource.FromStream(() => {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}
