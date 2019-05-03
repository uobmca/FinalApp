using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FinalApp.Views.Pages.IncomesList {
    public partial class IncomesListPage : ContentPage {
        public IncomesListPage() {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", async () => {
                await Navigation.PopModalAsync();
            }));
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {

        }
    }
}
