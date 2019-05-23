using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FinalApp.Models;

namespace FinalApp.Services {
    public partial class OCRDataExtractor : IOCRDataExtractor {

        // Patterns for matching "Generic Receipt" price labels
        private string[] genericReceiptPriceLabelPatterns = new string[] {
            "TOTAL PIECES",
            "NET BILL VAL",
            "TOTAL",
            "SUB TOTAL",
            "TOTALE",
            "TOTALE EURO",
            "VAL",
            "VALUE"
        };

        // Patterns for matching "Bill Type 01" price labels
        private string[] bill01PriceLabelPatterns = new string[] {
            "TOTAL CHARGES FOR THE PERIOD",
            "TOTAL CHARGES FOR",
            "TOTAL CHARGES"
        };

        // Patterns for matching "Bill Type 02" price labels
        private string[] bill02PriceLabelPatterns = new string[] {
            "TOTAL AMOUNT PAYABLE",
            "TOTAL AMOUNT",
            "TOTAL"
        };

        private string[] bill02ExpireDatePatterns = new string[] {
            "PAY BEFORE",
            "BEFORE"
        };

        public async Task<UserExpense> ExtractExpensesFromReceipt(RecognitionResult recognitionResult, Metadata imageMetadata) {
            double? extractedPrice = await HorizontalLabelPriceSearch(recognitionResult, imageMetadata, genericReceiptPriceLabelPatterns);
            if (!extractedPrice.HasValue) return null;
            return new UserExpense {
                Amount = extractedPrice.Value,
                Description = "Receipt expense",
                StartDate = DateTime.Now,
                ExpireDate = DateTime.Now
            };
        }

        public async Task<UserExpense> ExtractExpensesFromTypeOneBill(RecognitionResult recognitionResult, Metadata imageMetadata) {
            double? extractedPrice = await HorizontalLabelPriceSearch(recognitionResult, imageMetadata, bill01PriceLabelPatterns);
            if (!extractedPrice.HasValue) return null;
            return new UserExpense {
                Amount = extractedPrice.Value,
                Description = "Type one bill expense",
                StartDate = DateTime.Now,
                ExpireDate = DateTime.Now
            };
        }

        public async Task<UserExpense> ExtractExpensesFromTypeTwoBill(RecognitionResult recognitionResult, Metadata imageMetadata) {
            // Output attributes
            double amount = 0.0;
            DateTime expireDate = DateTime.Now;

            // Extract values
            double? extractedPrice = await VerticalLabelPriceSearch(recognitionResult, imageMetadata, bill02PriceLabelPatterns);
            DateTime? extractedDate = await VerticalLabelDateSearch(recognitionResult, imageMetadata, bill02ExpireDatePatterns);

            if (!extractedPrice.HasValue && ! extractedDate.HasValue) {
                return null;
            }    

            if (extractedPrice.HasValue) {
                amount = extractedPrice.Value;
            }

            if (extractedDate.HasValue) {
                expireDate = extractedDate.Value;
            }

            return new UserExpense {
                Amount = amount,
                ExpireDate = expireDate,
                Description = "Type two bill expense",
                StartDate = DateTime.Now
            };
        }

        private async Task<DateTime?> VerticalLabelDateSearch(RecognitionResult recognitionResult, Metadata imageMetadata, string[] patterns) {
            Dictionary<string, string> rawExpenses = new Dictionary<string, string>(); 

            List<TextRectangle> rectangles = recognitionResult.Lines
                .Select((line) => RectangleUtils.FromPointsArray(line))
                .ToList();

            // Zone 0
            var map = new RectanglesZoneMap(rectangles, imageMetadata);
            var dateRegex = new Regex(@"^\s*\d{2}\/\d{2}\/\d{4}\s*$", RegexOptions.IgnoreCase);

            bool found = false;
            int startingX = -1;
            int startingY = -1;
            for (int i = 0; i < map.VerticalZoneCount; i++) { 
                for (int k = 0; k < map.HorizontalZoneCount; k++ ) { 
                    if (TestForPriceLabel(map.MapMatrix[i,k], patterns)) {
                        found = true;
                        startingX = k;
                        startingY = i;
                        break;
                    }
                }
            }

            if (found) {
                int offsetX = startingX + 4;
                if (offsetX >= map.HorizontalZoneCount) {
                    offsetX = map.HorizontalZoneCount - 1;
                }

                for (int cY = startingY; cY < map.VerticalZoneCount; cY++) {
                    for (int cX = startingX; cX < offsetX; cX++) {
                        var cleanStr = map.MapMatrix[cY, cX].Trim().Replace(" ", "");
                        if(dateRegex.IsMatch(cleanStr)) {
                            try { 
                                var value = Convert.ToDateTime(cleanStr);
                                return value;
                            } catch(Exception e) {
                                Debug.Print(e.StackTrace);
                                break;
                            }
                        }

                    }    
                }
            }

            return null;
        }

        private async Task<double?> VerticalLabelPriceSearch(RecognitionResult recognitionResult, Metadata imageMetadata, string[] patterns) {
            Dictionary<string, string> rawExpenses = new Dictionary<string, string>(); 

            List<TextRectangle> rectangles = recognitionResult.Lines
                .Select((line) => RectangleUtils.FromPointsArray(line))
                .ToList();

            // Zone 0
            var map = new RectanglesZoneMap(rectangles, imageMetadata);
            var priceRegex = new Regex(@"^\s*(\W\D){0,1}\s*[\d,]+\.[\d]{2}\s*$", RegexOptions.IgnoreCase);

            bool found = false;
            int startingX = -1;
            int startingY = -1;
            for (int i = 0; i < map.VerticalZoneCount; i++) { 
                for (int k = 0; k < map.HorizontalZoneCount; k++ ) { 
                    if (TestForPriceLabel(map.MapMatrix[i,k], patterns)) {
                        found = true;
                        startingX = k;
                        startingY = i;
                        break;
                    }
                }
            }

            if (found) {
                int offsetX = startingX + 4;
                if (offsetX >= map.HorizontalZoneCount) {
                    offsetX = map.HorizontalZoneCount - 1;
                }

                for (int cY = startingY; cY < map.VerticalZoneCount; cY++) {
                    for (int cX = startingX; cX < offsetX; cX++) {
                        var cleanStr = map.MapMatrix[cY, cX].Trim().Replace(" ", "");
                        if(priceRegex.IsMatch(cleanStr)) {
                            try { 
                                var value = double.Parse(cleanStr, System.Globalization.NumberStyles.Currency, CultureInfo.GetCultureInfo("si-LK"));
                                return value;
                            } catch(Exception e) {
                                Debug.Print(e.StackTrace);
                                break;
                            }
                        }

                    }    
                }
            }

            return null;
        }

        private async Task<double?> HorizontalLabelPriceSearch(RecognitionResult recognitionResult, Metadata imageMetadata, string[] patterns) {

            Dictionary<string, string> rawExpenses = new Dictionary<string, string>(); 

            List<TextRectangle> rectangles = recognitionResult.Lines
                .Select((line) => RectangleUtils.FromPointsArray(line))
                .ToList();

            // Zone 0
            var map = new RectanglesZoneMap(rectangles, imageMetadata);
            var priceRegex = new Regex(@"^\s*(\W\D){0,1}\s*[\d,]+\.[\d]{2}\s*$", RegexOptions.IgnoreCase);
            bool found = false;
            int startingX = -1;
            int startingY = -1;
            for (int i = 0; i < map.VerticalZoneCount; i++) { 
                for (int k = 0; k < map.HorizontalZoneCount; k++ ) { 
                    if (TestForPriceLabel(map.MapMatrix[i,k], patterns)) {
                        found = true;
                        startingX = k;
                        startingY = i;
                        break;
                    }
                }
            }

            if (found) { 
                for (int i = startingX; i < map.HorizontalZoneCount; i++) {
                    var cleanStr = map.MapMatrix[startingY, i].Trim().Replace(" ", "");
                    if(priceRegex.IsMatch(cleanStr)) {
                        try { 
                            var value = double.Parse(cleanStr, System.Globalization.NumberStyles.Currency, CultureInfo.GetCultureInfo("si-LK"));
                            return value;
                        } catch(Exception e) {
                            Debug.Print(e.StackTrace);
                            break;
                        }
                    }
                }
            }

            return null;
        }

        private bool TestForPriceLabel(string str, string[] patterns) {
            var regexConds = GetPriceLabelRegexConds(patterns);

            foreach(Regex rx in regexConds) { 
                if(rx.IsMatch(str)) {
                    return true;
                }
            }

            return false;
        }

        private Regex[] GetPriceLabelRegexConds(string[] patterns) {
            return patterns.Select((pattern) => GetPriceLabelRegex(pattern)).ToArray();
        }

        private Regex GetPriceLabelRegex(string pattern) {
            return new Regex(string.Format(@"(\s*{0}\s*$)|(\s*{0}\s*:\s*)|(\s*{0}\s+.*)", pattern), RegexOptions.IgnoreCase);
        }

    }
}
