using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFLViewer.Model.Model.Interfaces
{
    public interface IPLFunctionModel
    {
        int Count { get; }
        IPointModel this[int index] { get; set; }
        IEnumerable<IPointModel> Points { get; }
        void Add(double x, double y);
        void Remove(IPointModel pointModel);
        
    }
}
