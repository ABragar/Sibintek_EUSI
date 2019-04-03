using Base.Map.Spatial;

namespace Base.Map.Helpers
{
    public static class MapHelper
    {
        public static bool InZoom(int? zoom, int? minZoom, int? maxZoom)
        {
            return (zoom.HasValue && !minZoom.HasValue && !maxZoom.HasValue) || MathHelper.Include(zoom, minZoom, maxZoom);
        }
    }
}