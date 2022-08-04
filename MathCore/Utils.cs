using MathCore.Primitives.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore
{
    public static class MathUtils
    {
        public static Func<double, double> GetLinearFunction(IPoint2D p1, IPoint2D p2)
        {
            return x => (x - p1.X) / (p2.X - p1.X) * (p2.Y - p1.Y) + p1.Y;
        }

        public static double GetDistance(double p1X, double p1Y, double p2X, double p2Y)
        {
            return Math.Sqrt(Math.Pow(p1X - p2X, 2) + Math.Pow((p1Y - p2Y), 2));
        }
    }
}
