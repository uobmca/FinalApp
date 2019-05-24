using Xamarin.Forms;

namespace FinalApp.Views.Pages.Tags {
    public class CategoryIcon {
        public Command<CategoryIcon> OnClickedCommand { get; set; }
        public Image IconImage { get; set; }
        public string IconSource { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public CategoryIcon(string iconSource, int row, int column) {
            IconImage = new Image {
                Source = iconSource,
                WidthRequest = 32,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Opacity = 0.25
            };
            IconSource = iconSource;
            Row = row;
            Column = column;

            SetupGestureRecognizers();
        }

        private void SetupGestureRecognizers() {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) => {
                if (OnClickedCommand == null) return;
                OnClickedCommand.Execute(this);
            };
            IconImage.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public static string[] DefaultSources = new string[] {
            "ic_tag_home",
            "ic_tag_work",
            "ic_tag_salary",
            "ic_tag_food",
            "ic_tag_cards",
            "ic_tag_beer",
            "ic_tag_car",
            "ic_tag_fun",
            "ic_tag_bills",
            "ic_tag_pets"
        };
    }
}
