using Base.Map.Spatial;
using System.Linq;

namespace Base.Map.Clustering
{
    internal class QuadBoundsProvider
    {
        private readonly QuardGridCache _cache;

        public QuadBoundsProvider(QuardGridCache cache = null)
        {
            _cache = cache;
        }

        public Bounds? FindBounds(IQueryable<IGeoObject> query)
        {
            Bounds bounds;

            if (_cache != null && _cache.TryGet(out bounds))
            {
                return bounds;
            }

            var result = query?.Where(x => x.Location.HasCenterPoint)
                .GroupBy(x => 1)
                .Select(x => new
                {
                    MinX = x.Min(p => p.Location.CenterPointX),
                    MinY = x.Min(p => p.Location.CenterPointY),
                    MaxX = x.Max(p => p.Location.CenterPointX),
                    MaxY = x.Max(p => p.Location.CenterPointY)
                }).FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            bounds = new Bounds();
            bounds.Extend(new Point(result.MinX, result.MinY));
            bounds.Extend(new Point(result.MaxX, result.MaxY));

            if (bounds.Equals(Bounds.Empty))
            {
                return null;
            }

            _cache?.TrySet(bounds);
            return bounds;
        }
    }
}