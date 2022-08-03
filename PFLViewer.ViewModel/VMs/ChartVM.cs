using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace PLFViewer.ViewModel.VMs
{
    public class ChartVM : ObservableObject
    {
        protected ObservableCollection<ISeries> _series = new ObservableCollection<ISeries>();

        public ChartVM()
        {
        }


        public ObservableCollection<ISeries> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public ICartesianAxis[] XAxes { get; set; } =
        {
            new Axis()
            {
                Name = "X",
                SeparatorsPaint = new SolidColorPaint(new SkiaSharp.SKColor(15,80,180)),
                LabelsPaint = new SolidColorPaint(new SkiaSharp.SKColor(15,80,180)),
            }
        };

        public ICartesianAxis[] YAxes { get; set; } =
        {
            new Axis()
            {
                Name = "Y"
            }
        };
    }
}