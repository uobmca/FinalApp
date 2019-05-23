using FinalApp.Models;
using Xamarin.Forms;

namespace FinalApp.Services {
    public class TextRectangle {
        public int HorizontalZoneNumber { get; set; } = -1;
        public int VerticalZoneNumber { get; set; } = -1;
        public Rectangle AvgBoundingBox { get; set; }
        public Line RectLine { get; set; }

        public TextRectangle(Rectangle avgBoundingBox, Line line) {
            this.AvgBoundingBox = avgBoundingBox;
            this.RectLine = line;
        }
    }
}
