using MathCore.Primitives.Interfaces;

namespace MathCore.Primitives
{
    public class Line2D : ILine2D
    {
        public IPoint2D Start { get; }
        public IPoint2D End { get; }
    }
}
