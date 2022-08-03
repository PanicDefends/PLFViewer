using Microsoft.Win32;
using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl
{
    public abstract class FileDialogServiceBuilder
    {
        public static FileDialogServiceBuilder OpenDialog => new OpenDialogServiceBuilder();
        public static FileDialogServiceBuilder SaveDialog => new SaveDialogServiceBuilder();


        protected FileDialog _fileDialog;

        public FileDialogServiceBuilder SetTitle(string title)
        {
            _fileDialog.Title = title;
            return this;
        }

        public FileDialogServiceBuilder SetFilter(string filter)
        {
            _fileDialog.Filter = filter;
            return this;
        }

        public FileDialogServiceBuilder SetDefaultExtension(string extension)
        {
            _fileDialog.DefaultExt = extension;
            return this;
        }

        public FileDialogServiceBuilder Reset()
        {
            _fileDialog.Reset();
            return this;
        }

        public FileDialogServiceBuilder SetInitialDir(string initialDir)
        {
            _fileDialog.InitialDirectory = initialDir;
            return this;
        }

        public IFileDialogService Build() => new Win32FileDialogService(_fileDialog);
    }

    internal class OpenDialogServiceBuilder : FileDialogServiceBuilder
    {
        public OpenDialogServiceBuilder()
        {
            _fileDialog = new OpenFileDialog();
        }
    }

    internal class SaveDialogServiceBuilder : FileDialogServiceBuilder
    {
        public SaveDialogServiceBuilder()
        {
            _fileDialog = new SaveFileDialog();
        }
    }
}
