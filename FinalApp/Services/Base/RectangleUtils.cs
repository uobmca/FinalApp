using System.Collections.Generic;
using System.Linq;
using FinalApp.Models;
using Xamarin.Forms;

namespace FinalApp.Services {


    public partial class OCRDataExtractor {
        private class RectangleUtils {
            public static TextRectangle FromPointsArray(Line line) {
                List<Point> points = new List<Point>();
                var pointsValues = line.BoundingBox;
                for (int i = 0; i < pointsValues.Count() - 1; i += 2) {
                    points.Add(new Point(pointsValues[i], pointsValues[i + 1]));
                }

                double xMin = double.MaxValue;
                double xMax = double.MinValue;
                double yMin = double.MaxValue;
                double yMax = double.MinValue;

                foreach (Point point in points) {
                    if (point.X < xMin) {
                        xMin = point.X;
                    }

                    if (point.X > xMax) {
                        xMax = point.X;
                    }

                    if (point.Y < yMin) {
                        yMin = point.Y;
                    }

                    if (point.Y > yMax) {
                        yMax = point.Y;
                    }
                }

                return new TextRectangle(new Rectangle(new Point(xMin, yMax), new Size(xMax - xMin, yMax - yMin)), line);
            }
        }

    }
}
