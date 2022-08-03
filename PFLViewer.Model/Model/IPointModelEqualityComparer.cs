using PFLViewer.Model.Model.Interfaces;
using System.Collections.Generic;

namespace PFLViewer.Model.Model
{
    internal class IPointModelEqualityComparer : IEqualityComparer<IPointModel>
    {
        public bool Equals(IPointModel p1, IPointModel p2)
        {
            if (p1 == null && p2 == null)
                return true;
            if (p1 == null ^ p2 == null)
                return false;

            return p1.X.Equals(p2.X) && p1.Y.Equals(p2.Y); 
        }

        public int GetHashCode(IPointModel obj)
        {
            if (obj == null)
                return 0;

            return (int)obj.X ^ (int)obj.Y;
        }
    }
}