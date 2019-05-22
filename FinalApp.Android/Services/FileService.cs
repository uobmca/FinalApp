using System;
using System.IO;
using Android.Content;
using Android.Support.V4.Content;
using Android.Webkit;
using Android.Widget;
using FinalApp.Droid.Services;
using FinalApp.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace FinalApp.Droid.Services {
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

            var appContext = CrossCurrentActivity.Current.AppContext;
            var context = CrossCurrentActivity.Current.Activity;

            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dirPath = Path.Combine(dirPath, "Reports");
            Java.IO.File file = new Java.IO.File(dirPath, fileName);

            if (!file.Exists()) {
                return;
            }

            Device.BeginInvokeOnMainThread(() => {
                Android.Net.Uri uri = FileProvider.GetUriForFile(context, context.PackageName + ".fileprovider", file);
                Intent intent = new Intent(Intent.ActionView);
                string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl((string)uri).ToLower());
                intent.SetDataAndType(uri, mimeType);

                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                try {
                    appContext.StartActivity(intent);
                } catch (Exception ex) {
                    Toast.MakeText(appContext, "No Application Available to View this file", ToastLength.Short).Show();
                }
            });
        }
    }
}
