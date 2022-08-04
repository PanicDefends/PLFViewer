using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PLFViewer.Common.Impl
{
    internal class Win32FileDialogService : IFileDialogService
    {
        protected FileDialog _fileDialog;
        public Win32FileDialogService(FileDialog fileDialog)
        {
            _fileDialog = fileDialog;
        }

        public string PickFilePath()
        {
            var result = _fileDialog.ShowDialog();
            var filePath = result == true ? _fileDialog.FileName : null;
            return filePath;
        }
    }
}
