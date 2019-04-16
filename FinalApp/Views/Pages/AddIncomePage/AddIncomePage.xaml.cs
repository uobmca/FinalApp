using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            insertManuallyOptionGrid.Opacity = 0.0;
            Device.BeginInvokeOnMainThread(() => {
                RunAnimations();
            });
        }

        protected override void OnAppearing() {
            base.OnAppearing();


        }

        private void RunAnimations() {
            takePictureOptionGrid.FadeTo(1.0, 500);
            takePictureOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
            insertManuallyOptionGrid.FadeTo(1.0, 500);
            insertManuallyOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
        }
    }
}
