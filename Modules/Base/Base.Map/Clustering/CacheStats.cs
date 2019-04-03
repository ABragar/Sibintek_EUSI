using System;

namespace Base.Map.Clustering
{
    public class CacheStats
    {
        public long Hits { get; set; }

        public long Miss { get; set; }

        public long Requests => Miss + Hits;

        public float HitRate => Requests > 0 ? (float)Math.Round(Hits * 100.0 / Requests, 2) : 0;

        public float MissRate => Requests > 0 ? (float)Math.Round(Miss * 100.0 / Requests, 2) : 0;
    }
}