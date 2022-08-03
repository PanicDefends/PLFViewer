using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PFLViewer.Model.Model;
using PFLViewer.Model.Model.Interfaces;
using PLFViewer.Common.Impl;
using PLFViewer.Common.Impl.Interfaces;
using PLFViewer.Common.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.ViewModel.VMs
{
    public class MainVM : ObservableObject
    {
        protected SerializationHelper _serializationHelper;

        protected readonly ChartVM _chartVM = new ChartVM();
        protected readonly ObservableCollection<PLFunctionVM> _functions = new ObservableCollection<PLFunctionVM>();
        protected PLFunctionVM _currentFunction;

        protected RelayCommand _addFunctionCommand;
        protected RelayCommand _addInversedFunctionCommand;
        protected RelayCommand _removeFunctionCommand;
        protected RelayCommand _loadFromFileCommand;
        protected RelayCommand _saveToFileCommand;


        public MainVM(ISerializationService serializationService)
        {
            _serializationHelper = new SerializationHelper(_functions, serializationService);
            _functions.CollectionChanged += OnFunctionCollectionChanged;
            OnClosing = DoOnClosing;
        }

        private void OnFunctionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _serializationHelper.HasUnsavedChanges = true;

            if (e.NewItems?.Count > 0)
            {
                foreach (PLFunctionVM item in e.NewItems)
                    item.Function.FunctionChanged += OnFunctionChanged;
            }

            if (e.OldItems?.Count > 0)
            {
                foreach (PLFunctionVM item in e.OldItems)
                    item.Function.FunctionChanged -= OnFunctionChanged;
            }
        }

        private void OnFunctionChanged(object sender, EventArgs e)
        {
            _serializationHelper.HasUnsavedChanges = true;
        }


        public event EventHandler WindowClosingRequested;
        public Func<bool> OnClosing { get; set; }

        public SerializationHelper SerializationHelper => _serializationHelper;
        public string Title => ResourcesEN.MainView_Title;
        public ChartVM ChartVM => _chartVM;
        public ObservableCollection<PLFunctionVM> Functions => _functions;

        public PLFunctionVM CurrentFunction
        {
            get => _currentFunction;
            set
            {
                _currentFunction = value;
                OnPropertyChanged();
                _addInversedFunctionCommand.NotifyCanExecuteChanged();
                _removeFunctionCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand AddFunctionCommand => _addFunctionCommand ??
            (_addFunctionCommand = new RelayCommand(() =>
            {
                AddFunction(new PLFunctionVM());
            }));

        protected void AddFunction(PLFunctionVM function)
        {
            _functions.Add(function);
            _chartVM.Series.Add(function.Series);
        }

        public RelayCommand AddInversedFunctionCommand => _addInversedFunctionCommand ??
            (_addInversedFunctionCommand = new RelayCommand(() =>
            {
                AddInversedFunction(CurrentFunction);
            },
                () =>
                {
                    return CurrentFunction != null;
                }));

        protected void AddInversedFunction(PLFunctionVM function)
        {
            var newFunctionVM = new PLFunctionVM(PLFunctionModel.GetInversed(function.Function));
            _functions.Add(newFunctionVM);
            _chartVM.Series.Add(newFunctionVM.Series);
        }

        public RelayCommand RemoveFunctionCommand => _removeFunctionCommand ??
            (_removeFunctionCommand = new RelayCommand(() =>
            {
                RemoveFunction(CurrentFunction);
            },
                () =>
                {
                    return CurrentFunction != null;
                }));

        protected void RemoveFunction(PLFunctionVM function)
        {
            _chartVM.Series.Remove(function.Series);
            _functions.Remove(function);
        }


        public RelayCommand LoadFromFileCommand => _loadFromFileCommand ??
            (_loadFromFileCommand = new RelayCommand(() =>
            {
                var data = _serializationHelper.LoadFromFile();
                Reinitialize(data);
            }));


        private void Reinitialize(IEnumerable<PLFunctionModel> data)
        {
            _functions.Clear();
            data.Select(f => new PLFunctionVM(f))
                .ToList()
                .ForEach(f => AddFunction(f));

            _serializationHelper.HasUnsavedChanges = false;
        }


        public RelayCommand SaveToFileCommand => _saveToFileCommand ??
            (_saveToFileCommand = new RelayCommand(() =>
            {
                _serializationHelper.SaveToFile();
            }));


        public void RequestWindowClosing() => WindowClosingRequested?.Invoke(this, EventArgs.Empty);
        public bool DoOnClosing()
        {
            var dialogService = MessageDialogFactory.GetMessageDialog(
                "Unsaved changes", "Do you want to save changes?", DialogButtonSet.YesNoCancel);
            var result = dialogService.ShowMessage();

            if (result == null)
                return false;
            else if (result == true)
            {
                _serializationHelper.SaveToFile();
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
