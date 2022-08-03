using MathCore.Primitives;
using MathCore.Primitives.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PFLViewer.Model.Model.Interfaces;
using PLFViewer.Common.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFLViewer.Model.Model
{
    public class PointModel : ObservableValuesObject, IPointModel
    {
        protected IPoint2D _point;

        public PointModel(double x, double y)
        {
            _point = new Point2D(x, y);
        }

        public double X
        {
            get { return _point.X; }
            set
            {
                var oldValue = _point.X;
                _point = new Point2D(value, Y);
                OnPropertyChanged();
                OnPropertyValueChanged(oldValue, value);
            }
        }

        public double Y
        {
            get { return _point.Y; }
            set
            {
                var oldValue = _point.Y;
                _point = new Point2D(X, value);
                OnPropertyChanged();
                OnPropertyValueChanged(oldValue, value);
            }
        }
    }
}
