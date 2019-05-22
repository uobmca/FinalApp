using System;
using System.IO;
using FinalApp.iOS.Services;
using FinalApp.Services;
using Foundation;
using QuickLook;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace FinalApp.iOS.Services {
    public class FileService : IFileService {
        // Saves data stream to device
        public void SaveDataStream(string name, Stream data, string location = "temp") {
            // Create documents path
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, location);
            Directory.CreateDirectory(documentsPath);

            // Resolve file path
            string filePath = Path.Combine(documentsPath, name);

            // Write byte data
            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate)) {
                using (data) {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }

        public void ShowDocument(string fileName) {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dirPath = Path.Combine(dirPath, "Reports");

            var filename = Path.Combine(dirPath, fileName);
            FileInfo fi = new FileInfo(filename);

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                QLPreviewController previewController = new QLPreviewController();
                previewController.DataSource = new PDFPreviewControllerDataSource(fi.FullName, fi.Name);
                UINavigationController controller = FindNavigationController();
                if (controller != null)
                    controller.PresentViewController(previewController, true, null);
            });

        }

        private UINavigationController FindNavigationController() {
            foreach (var window in UIApplication.SharedApplication.Windows) {
                if (window.RootViewController.NavigationController != null)
                    return window.RootViewController.NavigationController;
                else {
                    UINavigationController val = CheckSubs(window.RootViewController.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }

            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers) {
            foreach (var controller in controllers) {
                if (controller.NavigationController != null)
                    return controller.NavigationController;
                else {
                    UINavigationController val = CheckSubs(controller.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }
            return null;
        }
    }

    public class PDFItem : QLPreviewItem {
        string title;
        string uri;

        public PDFItem(string title, string uri) {
            this.title = title;
            this.uri = uri;
        }

        public override string ItemTitle {
            get { return title; }
        }

        public override NSUrl ItemUrl {
            get { return NSUrl.FromFilename(uri); }
        }
    }

    public class PDFPreviewControllerDataSource : QLPreviewControllerDataSource {
        string url = "";
        string filename = "";

        public PDFPreviewControllerDataSource(string url, string filename) {
            this.url = url;
            this.filename = filename;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index) {
            return (IQLPreviewItem)new PDFItem(filename, url);
        }

        public override nint PreviewItemCount(QLPreviewController controller) {
            return 1;
        }
    }
}
