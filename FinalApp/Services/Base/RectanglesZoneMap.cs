using System;
using System.Collections.Generic;
using System.Linq;
using FinalApp.Models;

namespace FinalApp.Services {
    public class RectanglesZoneMap {

        // The percentage of tolerance when dealing with classifying bounding boxes zone
        private const float kHorizontalTolerancePercentage = 0.07f;
        private const float kVerticalTolerancePercentage = 0.06f;

        public List<List<TextRectangle>> HorizontalZoneRectangles { get; set; }
        public List<List<TextRectangle>> VerticalZoneRectangles { get; set; }
        private List<TextRectangle> rectangles;
        private Metadata imageMetaData;

        public int HorizontalZoneCount { get; private set; }
        public int VerticalZoneCount { get; private set; }

        public string[,] MapMatrix { get; set; }

        public RectanglesZoneMap(List<TextRectangle> rectangles, Metadata imageMetadata) {
            this.rectangles = rectangles;
            this.imageMetaData = imageMetadata;
            RebuildMapMatrix();
        }

        private void RebuildMapMatrix() {

            var hStep = imageMetaData.Width * kHorizontalTolerancePercentage;
            var yStep = imageMetaData.Height * kVerticalTolerancePercentage;

            int hSize = (int)Math.Ceiling(imageMetaData.Width / hStep);
            int ySize = (int)Math.Ceiling(imageMetaData.Height / yStep);

            HorizontalZoneCount = hSize;
            VerticalZoneCount = ySize;

            var matrix = new string[ySize, hSize];

            for (int i = 0; i < ySize; i++) {
                for (int k = 0; k < hSize; k++) {
                    matrix[i, k] = "";
                }
            }

            foreach (TextRectangle rectangle in rectangles) {

                int x = (int)Math.Round(rectangle.AvgBoundingBox.X / hStep);
                int y = (int)Math.Round(rectangle.AvgBoundingBox.Y / yStep);

                x = x < 0 ? 0 : x;
                x = x > hSize - 1 ? hSize - 1 : x;

                y = y < 0 ? 0 : y;
                y = y > ySize - 1 ? ySize - 1 : y;

                matrix[y, x] = rectangle.RectLine.Text;
            }

            MapMatrix = matrix;


        }

        public TextRectangle GetRectangleForZone(int hZone, int vZone) {
            TextRectangle firstMatchTry = HorizontalZoneRectangles[hZone].FirstOrDefault((rect) => rect.VerticalZoneNumber == vZone);
            if (firstMatchTry == null) {
                TextRectangle secondMatchTry = VerticalZoneRectangles[vZone].FirstOrDefault((rect) => rect.HorizontalZoneNumber == hZone);
                if (secondMatchTry == null) {
                    return null;
                } else {
                    return secondMatchTry;
                }
            } else {
                return firstMatchTry;
            }
        }

        private static List<List<TextRectangle>> GetHorizontalZoneTextRectangles(List<TextRectangle> rectangles, Metadata imageMetadata) {
            rectangles = rectangles.OrderBy((rect) => rect.AvgBoundingBox.X).ToList();
            List<List<TextRectangle>> horizontalZoneRectangles = new List<List<TextRectangle>>();
            horizontalZoneRectangles.Add(new List<TextRectangle>());
            int zoneNumber = 0;
            for (int i = 0; i < rectangles.Count() - 1; i++) {
                var current = rectangles[i];
                var other = rectangles[i + 1];
                var currentX = current.AvgBoundingBox.X;
                var otherX = other.AvgBoundingBox.X;

                var tolerance = imageMetadata.Width * kHorizontalTolerancePercentage;
                if (Math.Abs(otherX - currentX) < tolerance) {

                    if (!horizontalZoneRectangles.Last().Contains(current)) {
                        current.HorizontalZoneNumber = zoneNumber;
                        horizontalZoneRectangles.Last().Add(current);
                    }
                    other.HorizontalZoneNumber = zoneNumber;
                    horizontalZoneRectangles.Last().Add(other);
                } else {
                    if (horizontalZoneRectangles.Last().Any()) {
                        horizontalZoneRectangles.Add(new List<TextRectangle>());
                        zoneNumber++;
                    } else {
                        current.VerticalZoneNumber = zoneNumber;
                        horizontalZoneRectangles.Last().Add(current);
                    }
                }
            }
            return horizontalZoneRectangles;
        }

        private static List<List<TextRectangle>> GetVerticalZoneTextRectangles(List<TextRectangle> rectangles, Metadata imageMetadata) {
            rectangles = rectangles.OrderBy((rect) => rect.AvgBoundingBox.Y).ToList();
            List<List<TextRectangle>> verticalZoneRectangles = new List<List<TextRectangle>>();

            // Zone 0
            verticalZoneRectangles.Add(new List<TextRectangle>());
            int zoneNumber = 0;
            for (int i = 0; i < rectangles.Count() - 1; i++) {
                var current = rectangles[i];
                var other = rectangles[i + 1];
                var currentY = current.AvgBoundingBox.Y;
                var otherY = other.AvgBoundingBox.Y;

                var tolerance = imageMetadata.Height * kVerticalTolerancePercentage;
                if (Math.Abs(otherY - currentY) < tolerance) {

                    if (!verticalZoneRectangles.Last().Contains(current)) {
                        current.VerticalZoneNumber = zoneNumber;
                        verticalZoneRectangles.Last().Add(current);
                    }
                    other.VerticalZoneNumber = zoneNumber;
                    verticalZoneRectangles.Last().Add(other);
                } else {
                    if (verticalZoneRectangles.Last().Any()) {
                        verticalZoneRectangles.Add(new List<TextRectangle>());
                        zoneNumber++;
                    } else {
                        current.VerticalZoneNumber = zoneNumber;
                        verticalZoneRectangles.Last().Add(current);
                    }
                }
            }
            return verticalZoneRectangles;
        }


    }
}
