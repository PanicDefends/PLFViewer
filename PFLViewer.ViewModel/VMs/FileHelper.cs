using System;
using System.IO;

namespace PLFViewer.ViewModel.VMs
{
    public class FileHelper
    {
        public readonly string APP_DATA_DIR = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public const string APP_NAME = "PLFViewer";
        public readonly string FUNC_DB_DEFAULT_DIR;
        public const string EXTENSION = "fjson";
        public readonly string FILTER = $"Function json files (*.{EXTENSION})|*.{EXTENSION}|All files(*.*)|*.*";

        public FileHelper()
        {
            FUNC_DB_DEFAULT_DIR = Path.Combine(APP_DATA_DIR, APP_NAME);

            if (!Directory.Exists(FUNC_DB_DEFAULT_DIR))
                Directory.CreateDirectory(FUNC_DB_DEFAULT_DIR);
        }
    }
}