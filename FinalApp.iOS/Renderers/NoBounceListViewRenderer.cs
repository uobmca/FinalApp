using System;
using FinalApp.iOS.Renderers;
using FinalApp.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FinalApp.Views.Base.NoBounceListView), typeof(ListViewRenderer))]
namespace FinalApp.iOS.Renderers {
    public class NoBounceListView : ListViewRenderer {

        public NoBounceListView() {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e) {
            base.OnElementChanged(e);
            DisableBounce();
        }

        private void DisableBounce() {
            if (Element != null) {
                Control.AlwaysBounceVertical = false;
                Control.Bounces = false;
            }
        }
    }
}
