using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.AddIncomePage {
    public partial class AddIncomePage : ContentPage {

        private Task takePictureAnimation;
        private Task insertManuallyAnimation;

        public AddIncomePage() {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI() {
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", () => {
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            
            }));
            takePictureOptionGrid.Opacity = 0.0;
            pickPictureOptionGrid.Opacity = 0.0;
            insertManuallyOptionGrid.Opacity = 0.0;


            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) => {

                if (sender is Grid grid) {
                    grid.ToString();
                    ShowOptionSelection(grid);
                }

                if (sender == takePictureOptionGrid) {
                    await GoToTakePicturePage();
                    HideOptionSelection(takePictureOptionGrid);
                }

                if (sender == pickPictureOptionGrid) {
                    await OpenPictureGallery();
                    HideOptionSelection(pickPictureOptionGrid);
                }
            };

            takePictureOptionGrid.GestureRecognizers.Add(tapGestureRecognizer);
            pickPictureOptionGrid.GestureRecognizers.Add(tapGestureRecognizer);
            insertManuallyOptionGrid.GestureRecognizers.Add(tapGestureRecognizer);

            Device.BeginInvokeOnMainThread(() => {
                RunAnimations();
            });
        }

        private async Task OpenPictureGallery() {
            await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
                PhotoSize = PhotoSize.Medium
            });
        }

        private async Task GoToTakePicturePage() {
            await Navigation.PushAsync(new TakePicture.TakePicturePage());
        }

        private void ShowOptionSelection(Grid grid) {
            Device.BeginInvokeOnMainThread(() => {
                grid.BackgroundColor = Color.FromHex("#e2e2e2");
            });
        }

        private void HideOptionSelection(Grid grid) {
            Device.BeginInvokeOnMainThread(() => {
                grid.BackgroundColor = Color.Transparent;
            });
        }

        private void RunAnimations() {
            takePictureOptionGrid.FadeTo(1.0, 500);
            takePictureOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
            pickPictureOptionGrid.FadeTo(1.0, 500);
            pickPictureOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
            insertManuallyOptionGrid.FadeTo(1.0, 500);
            insertManuallyOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
        }
    }
}
