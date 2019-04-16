using System;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class AnalyzePicturePageViewModel : BindableObject {
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
    }
}
