using LiveChartsCore.SkiaSharpView;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PFLViewer.Model.Model;
using PFLViewer.Model.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.ViewModel.VMs
{
    public class PLFunctionVM : ObservableObject
    {
        protected string _displayString;
        protected PLFunctionModel _function;
        protected LineSeries<IPointModel> _series;

        protected double _newPointX;
        protected double _newPointY;
        protected RelayCommand _addPointCommand;
        protected RelayCommand<IPointModel> _removePointCommand;

        public PLFunctionVM()
            : this(new PLFunctionModel()) { }
        public PLFunctionVM(IPLFunctionModel function)
        {
            _displayString = "#" + this.GetHashCode().ToString();
            Function = new PLFunctionModel(function.Points);
            _series = BuildSeries();
            _series.Values = Function.Points;
            Function.PropertyChanged += OnFunctionPropertyChanged;
            Function.CollectionChanged += OnPointsCollectionChanged;
        }

        protected virtual LineSeries<IPointModel> BuildSeries()
        {
            var series = new LineSeries<IPointModel>()
            {
                LineSmoothness = 0,
            };

            return series;
        }
        private void OnFunctionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IPLFunctionModel function = null;
            if (e.PropertyName == nameof(function.Points))
            {
                _series.Values = _function.Points;
                DisplayString = _function.Count.ToString();
            }

        }
        private void OnPointsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DisplayString = _function.Count.ToString();
        }


        public string DisplayString
        {
            get => _displayString;
            set
            {
                _displayString = string.Format("#{0} [{1}", this.GetHashCode(), value);
                OnPropertyChanged();
            }
        }

        public PLFunctionModel Function
        {
            get => _function;
            set
            {
                _function = value;
                OnPropertyChanged();
            }
        }

        public LineSeries<IPointModel> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public double NewPointX
        {
            get => _newPointX;
            set
            {
                _newPointX = value;
                OnPropertyChanged();
            }
        }
        public double NewPointY
        {
            get => _newPointY;
            set
            {
                _newPointY = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddPointCommand => _addPointCommand ??
            (_addPointCommand = new RelayCommand(() =>
            {
                _function.Add(NewPointX, NewPointY);
            }));

        public RelayCommand<IPointModel> RemovePointCommand => _removePointCommand ??
            (_removePointCommand = new RelayCommand<IPointModel>(p =>
            {
                _function.Remove(p);
            }));
    }
}
