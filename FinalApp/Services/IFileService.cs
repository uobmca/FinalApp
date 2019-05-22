using System;
using System.IO;

namespace FinalApp.Services {
    public interface IFileService {
        void SaveDataStream(string name, Stream data, string location = "temp");
        void ShowDocument(string fileName);
    }
}
