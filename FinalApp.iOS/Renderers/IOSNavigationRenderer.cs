using System;
using FinalApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(IOSNavigationRenderer))]
namespace FinalApp.iOS.Renderers {
    public class IOSNavigationRenderer : PageRenderer {
        public IOSNavigationRenderer() {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.VisualElementChangedEventArgs e) {
            base.OnElementChanged(e);
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
        }

        private void SetupNavbar() {
            var navbar = NavigationController.NavigationBar;

            if (navbar != null) {
                navbar.BarTintColor = new UIColor(red: 0.54f, green: 0.67f, blue: 0.78f, alpha: 1.0f);
                navbar.TintColor = UIColor.Black;
            }
        }
    }
}
