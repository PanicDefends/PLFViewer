using Microsoft.Toolkit.Mvvm.Input;
using PFLViewer.Model.Model;
using PLFViewer.Common.Impl;
using PLFViewer.Common.Impl.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PLFViewer.ViewModel.VMs
{
    public class SerializationHelper
    {
        protected ISerializationService _serializationService;
        private ObservableCollection<PLFunctionVM> _functions;


        public SerializationHelper(ObservableCollection<PLFunctionVM> functions, ISerializationService serializationService)
        {
            _functions = functions;
            _serializationService = serializationService;
        }


        public FileHelper FileHelper { get; } = new FileHelper();
        public bool HasUnsavedChanges { get; set; }


        public IEnumerable<PLFunctionModel> LoadFromFile()
        {
            var dialogProvider = FileDialogServiceBuilder.OpenDialog
                .SetTitle("Select proper .fjson file...")
                .SetInitialDir(FileHelper.FUNC_DB_DEFAULT_DIR)
                .SetDefaultExtension(FileHelper.EXTENSION)
                .SetFilter(FileHelper.FILTER)
                .Build();

            var path = dialogProvider.PickFilePath();
            if (!File.Exists(path))
                return null;

            var data = _serializationService.Deserialize<IEnumerable<PLFunctionModel>>(path);
            if (data == null)
                return null;

            return data;
        }


        public void SaveToFile()
        {
            var dialogProvider = FileDialogServiceBuilder.SaveDialog
                .SetTitle("Save functions to .fjson file...")
                .SetInitialDir(FileHelper.FUNC_DB_DEFAULT_DIR)
                .SetDefaultExtension(FileHelper.EXTENSION)
                .SetFilter(FileHelper.FILTER)
                .Build();

            var path = dialogProvider.PickFilePath();
            if (string.IsNullOrEmpty(path))
                return;

            path = Path.ChangeExtension(path, FileHelper.EXTENSION);
            var data = PrepareDataToSerialize(_functions);
            var succeeded = _serializationService.Serialize(data, path);
            if (succeeded)
                HasUnsavedChanges = false;
        }

        private IEnumerable<PLFunctionModel> PrepareDataToSerialize(ObservableCollection<PLFunctionVM> functions)
        {
            return functions.Select(f => f.Function);
        }
    }
}