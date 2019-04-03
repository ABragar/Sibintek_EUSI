namespace Base.Map.Spatial
{
    public class AffineTransformation : ITransformation
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly double _d;

        public AffineTransformation(double a, double b, double c, double d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public Point Transform(Point point, double scale)
        {
            scale = scale < 0 || scale > 0 ? scale : 1;
            point.X = scale * (_a * point.X + _b);
            point.Y = scale * (_c * point.Y + _d);
            return point;
        }

        public Point Untransform(Point point, double scale)
        {
            scale = scale < 0 || scale > 0 ? scale : 1;
            point.X = (point.X / scale - _b) / _a;
            point.Y = (point.Y / scale - _d) / _c;
            return point;
        }
    }
}