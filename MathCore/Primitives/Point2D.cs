using MathCore.Primitives.Interfaces;

namespace MathCore.Primitives
{
    public struct Point2D : IPoint2D
    {
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }
}
