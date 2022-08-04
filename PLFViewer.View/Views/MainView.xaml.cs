using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.WPF;
using MathCore;
using PFLViewer.Model.Model.Interfaces;
using PLFViewer.Common.Impl;
using PLFViewer.ViewModel.VMs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLFViewer.View.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private ChartPoint _movingPoint;
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainVM(SerializationServiceFactory.Instance);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var vm = DataContext as MainVM;

            if (!vm.SerializationHelper.HasUnsavedChanges)
                return;

            var approveClosing = vm.OnClosing?.Invoke();
            e.Cancel = approveClosing != true;
        }


        private void Chart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (MainVM)DataContext;
            var currentFunction = viewModel.CurrentFunction;
            if (currentFunction == null)
                return;

            var chart = (CartesianChart)FindName("Chart");

            var clickPoint = e.GetPosition(chart);
            var scaledClickPoint = chart.ScaleUIPoint(new LvcPoint((float)clickPoint.X, (float)clickPoint.Y));

            var x = scaledClickPoint[0];
            var y = scaledClickPoint[1];

            currentFunction.Function.Add(x, y);
        }

        private void Chart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (MainVM)DataContext;
            var currentFunction = viewModel.CurrentFunction;
            if (currentFunction == null)
                return;

            var chart = (CartesianChart)FindName("Chart");
            var chartPoints = currentFunction.Series.ActivePoints;
            var clickPoint = e.GetPosition(chart);
            var scaledClickPoint = chart.ScaleUIPoint(new LvcPoint((float)clickPoint.X, (float)clickPoint.Y));
            var clickedPointX = scaledClickPoint[0];
            var clickedPointY = scaledClickPoint[1];

            var nearestPoint = chartPoints
                .Where(p => MathUtils.GetDistance(p.SecondaryValue, p.PrimaryValue, clickedPointX, clickedPointY) < 0.2)
                .OrderBy(p => MathUtils.GetDistance(p.SecondaryValue, p.PrimaryValue, clickedPointX, clickedPointY))
                .FirstOrDefault();

            if (nearestPoint == null)
                return;

            _movingPoint = nearestPoint;
            this.MouseMove += Chart_MouseMove;
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (_movingPoint == null || e.LeftButton != MouseButtonState.Pressed)
                return;

            var viewModel = (MainVM)DataContext;
            var currentFunction = viewModel.CurrentFunction;
            if (currentFunction == null)
                return;

            var chart = (CartesianChart)FindName("Chart");
            var chartPoints = currentFunction.Series.ActivePoints;
            var clickPoint = e.GetPosition(chart);
            var scaledClickPoint = chart.ScaleUIPoint(new LvcPoint((float)clickPoint.X, (float)clickPoint.Y));
            var draggedPointX = scaledClickPoint[0];
            var draggedPointY = scaledClickPoint[1];

            _movingPoint.SecondaryValue = draggedPointX;
            _movingPoint.PrimaryValue = draggedPointY;
        }

        private void Chart_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_movingPoint == null)
                return;

            this.MouseMove -= Chart_MouseMove;
            var movingPoint = _movingPoint;
            _movingPoint = null;

            var viewModel = (MainVM)DataContext;
            var currentFunction = viewModel.CurrentFunction;
            if (currentFunction == null)
                return;

            var chart = (CartesianChart)FindName("Chart");
            var chartPoints = currentFunction.Series.ActivePoints;
            var clickPoint = e.GetPosition(chart);
            var scaledClickPoint = chart.ScaleUIPoint(new LvcPoint((float)clickPoint.X, (float)clickPoint.Y));
            var draggedPointX = scaledClickPoint[0];
            var draggedPointY = scaledClickPoint[1];

            var pointModel = ((IPointModel)movingPoint.Context.DataSource);
            pointModel.X = draggedPointX;
            pointModel.Y = draggedPointY;
        }
    }
}
