using MathCore.Primitives.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore
{
    public static class Utils
    {
        public static class MathUtils
        {
            public static Func<double, double> GetLinearFunction(IPoint2D p1, IPoint2D p2)
            {
                return x => (x - p1.X) / (p2.X - p1.X) * (p2.Y - p1.Y) + p1.Y;
            }
        }
    }
}
