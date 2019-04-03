using System;

namespace Base.Map.Spatial
{
    public static class MathHelper
    {
        public const double Deg2Rad = Math.PI / 180.0;
        public const double Rad2Deg = 180.0 / Math.PI;
        public const double PI4 = Math.PI / 4.0;
        public const double PI2 = Math.PI / 2.0;
        public static readonly double LN2 = Math.Log(2);

        public static int Clamp(int value, int min, int max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

        public static bool Include(int? value, int? rangeStart, int? rangeEnd)
        {
            if (!value.HasValue || (!rangeStart.HasValue && !rangeEnd.HasValue))
            {
                return false;
            }

            return !((rangeStart.HasValue && value.Value < rangeStart.Value) ||
                   (rangeEnd.HasValue && value.Value > rangeEnd.Value));
        }
    }
}