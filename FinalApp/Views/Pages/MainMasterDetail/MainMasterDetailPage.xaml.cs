using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalApp.Views.Pages.MainMasterDetail {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPage : MasterDetailPage {
        MasterDetailPage mdPage;
        Color origContentBgColor;
        Color origPageBgColor;

        public MainMasterDetailPage() {
            InitializeComponent();
            SetupUI();

            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void SetupUI() {

            mdPage = this;
            IsPresentedChanged += (object sender, EventArgs e) => {
                if (Device.RuntimePlatform == Device.iOS) {
                    Page currentPage = mdPage.Detail;
                    NavigationPage navPage = currentPage as NavigationPage;
                    ContentPage detailPage = navPage.CurrentPage as ContentPage;

                    if (detailPage == null) {
                        if (navPage.CurrentPage is TabbedPage tabbedPage) { 
                            if (tabbedPage.CurrentPage is NavigationPage tabNavPage) {
                                if (tabNavPage.CurrentPage is ContentPage contentPage) {
                                    detailPage = contentPage;
                                }
                            }
                        }
                    }

                    if (detailPage == null) return;

                    if (mdPage.IsPresented) {
                        origPageBgColor = detailPage.BackgroundColor;
                        origContentBgColor = detailPage.Content.BackgroundColor;

                        detailPage.BackgroundColor = Color.Black;
                        detailPage.Content.FadeTo(0.5);

                        if (detailPage.Content.BackgroundColor == Color.Default) {
                            detailPage.Content.BackgroundColor = Color.White;
                        }

                    } else {
                        detailPage.BackgroundColor = origPageBgColor;
                        detailPage.Content.BackgroundColor = origContentBgColor;
                        detailPage.Content.FadeTo(1.0);
                    }
                }
            };
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as MainMasterDetailPageMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}
