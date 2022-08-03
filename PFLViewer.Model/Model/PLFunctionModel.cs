using Microsoft.Toolkit.Mvvm.ComponentModel;
using PFLViewer.Model.Model;
using PFLViewer.Model.Model.Interfaces;
using PLFViewer.Common.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFLViewer.Model.Model
{
    public class PLFunctionModel : ObservableObject, IPLFunctionModel, INotifyCollectionChanged
    {
        public static IPLFunctionModel GetInversed(IPLFunctionModel function)
        {
            return new PLFunctionModel(function.Points.Select(p => new PointModel(p.Y, p.X)));
        }


        protected ObservableCollection<IPointModel> _points;



        public PLFunctionModel(IEnumerable<IPointModel> points)
        {
            Points = new ObservableCollection<IPointModel>(points);
            SortPoints();
        }

        public PLFunctionModel()
        {
            Points = new ObservableCollection<IPointModel>();
        }


        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler FunctionChanged;
        public virtual int Count => _points.Count;
        public virtual IPointModel this[int index]
        {
            get => _points[index];
            set
            {
                if (_points[index] == value)
                    return;

                _points[index] = value;
            }
        }
        public virtual IEnumerable<IPointModel> Points
        {
            get => _points;
            protected set
            {
                if (_points != null)
                    _points.CollectionChanged -= OnPointsCollectionChanged;
                _points = new ObservableCollection<IPointModel>(value);
                _points.CollectionChanged += OnPointsCollectionChanged;
                OnPropertyChanged();
            }
        }



        public virtual void Add(double x, double y)
        {
            var newPoint = new PointModel(x, y);
            newPoint.PropertyValueChanged += OnPointPropertyValueChanged;
            _points.Add(newPoint);
        }

        public virtual void Remove(IPointModel pointModel)
        {
            if (!_points.Contains(pointModel))
                return;

            ((PointModel)pointModel).PropertyValueChanged -= OnPointPropertyValueChanged;
            _points.Remove(pointModel);
        }

        protected virtual void OnPointPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (!(sender is IPointModel))
                throw new ArgumentException(nameof(sender));

            FunctionChanged?.Invoke(this, EventArgs.Empty);

            if (!IsXPropertyName(e.PropertyName))
            {
                SortPoints();
                return;
            }

            double? oldValue = e.OldValue != null ? ((double)e.OldValue) : null;
            var pointEditInfo = new PointEditInfo((IPointModel)sender, oldValue);
            SortPoints(new IPointModelComparer(_points, new[] { pointEditInfo }));
        }

        private bool IsXPropertyName(string propertyName)
        {
            IPointModel point = null;
            return propertyName.Equals(nameof(point.X));
        }

        private void OnPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(sender, e);
            FunctionChanged?.Invoke(this, EventArgs.Empty);

            IComparer<IPointModel> comparer = null;
            List<PointEditInfo> pointEditInfos = null;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    pointEditInfos = e.NewItems.Cast<IPointModel>().Select(p => new PointEditInfo(p)).ToList();
                    comparer = new IPointModelComparer(_points, pointEditInfos);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var oldPoints = e.OldItems.Cast<IPointModel>().ToArray();
                    var newPoints = e.NewItems.Cast<IPointModel>().ToArray();
                    pointEditInfos = new List<PointEditInfo>();
                    for (int i = 0; i < oldPoints.Length; i++)
                    {
                        pointEditInfos.Add(new PointEditInfo(newPoints[0], oldPoints[0].X));
                    }
                    comparer = new IPointModelComparer(_points, pointEditInfos);
                    break;

                default:
                    return;
            }

            SortPoints(comparer);
        }

        protected virtual void SortPoints(IComparer<IPointModel> comparer = null)
        {
            Points = comparer == null
                ? _points.OrderBy(p => p.X).Distinct(new IPointModelEqualityComparer())
                : _points.OrderBy(p => p, comparer).Distinct(new IPointModelEqualityComparer());
        }
    }
}
