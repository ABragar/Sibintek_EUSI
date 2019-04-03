using Base.Map.MapObjects;
using Base.Map.Spatial;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Base.Map.Clustering
{
    internal class QuardGridCache
    {
        private static readonly CacheEntry _nullEntry = new CacheEntry();
        private readonly ConcurrentDictionary<CacheKey, CacheEntry> _store;

        public CacheGroup Group { get; }

        public QuardGridCache(CacheSettings cacheSettings)
        {
            if (cacheSettings == null)
            {
                throw new ArgumentNullException(nameof(cacheSettings));
            }

            if (string.IsNullOrEmpty(cacheSettings.GroupKey))
            {
                throw new ArgumentNullException(nameof(cacheSettings.GroupKey));
            }

            Group = new CacheGroup(cacheSettings.GroupKey, cacheSettings.Type, cacheSettings.GroupTitle);
            _store = CacheStoreManager.GetStore(Group);
        }

        public bool TryGet(int x, int y, int z, out GeoObjectBase value)
        {
            CacheEntry cacheEntry;

            if (!_store.TryGetValue(new CacheKey(x, y, z), out cacheEntry))
            {
                value = null;
                return false;
            }

            value = cacheEntry.Value;
            return true;
        }

        public void Insert(int x, int y, int z, GeoObjectBase value)
        {
            var cacheEntry = value != null ? new CacheEntry(value) : _nullEntry;
            _store.AddOrUpdate(new CacheKey(x, y, z), cacheEntry, (key, old) => cacheEntry);
        }

        public bool TryGet(out Bounds constraintBounds)
        {
            CacheGroupSettings settings;

            if (!CacheSettingsManager.TryGet(Group, out settings))
            {
                constraintBounds = Bounds.Empty;
                return false;
            }

            constraintBounds = settings.ConstraintBounds;
            return true;
        }

        public bool TrySet(Bounds constraintBounds)
        {
            CacheSettingsManager.Insert(Group, new CacheGroupSettings(constraintBounds));
            return true;
        }

        public bool Remove(int x, int y, int z)
        {
            CacheEntry value;
            return _store.TryRemove(new CacheKey(x, y, z), out value);
        }

        public void RemoveSettings()
        {
            CacheSettingsManager.Clear(Group);
        }

        public static void Clear(string groupKey)
        {
            var group = new CacheGroup(groupKey, null, null);
            CacheStoreManager.ClearStore(group);
            CacheSettingsManager.Clear(group);
            GC.Collect();
        }

        public static void ClearAll()
        {
            CacheStoreManager.ClearAllStores();
            CacheSettingsManager.ClearAll();
            CacheStatistics.Reset();
            GC.Collect();
        }

        public static bool HasGroup(string groupKey)
        {
            var group = new CacheGroup(groupKey,null,null);
            return CacheStoreManager.ContainsGroup(group);
        }

        public static IEnumerable<CacheGroup> GetGroups()
        {
            return CacheStoreManager.GetAllGroups();
        }

        public static CacheInfo GetInfo(string groupKey)
        {
            var group = GetGroups().FirstOrDefault(x => x.Key == groupKey);

            if (group == null)
            {
                return null;
            }

            return new CacheInfo
            {
                Group = group,
                CachedItems = CacheStoreManager.GetCount(group),
                UsedMemorySize = CacheStoreManager.GetMemorySize(group)
            };
        }

        public void Miss()
        {
            Interlocked.Increment(ref CacheStatistics.MissCount);
        }

        public void Hit()
        {
            Interlocked.Increment(ref CacheStatistics.HitCount);
        }

        public static void ResetStats()
        {
            CacheStatistics.Reset();
        }

        public static CacheStats GetStats()
        {
            return new CacheStats
            {
                Hits = CacheStatistics.HitCount,
                Miss = CacheStatistics.MissCount
            };
        }

        private static class CacheStoreManager
        {
            private static readonly ConcurrentDictionary<CacheGroup, ConcurrentDictionary<CacheKey, CacheEntry>> _stores = new ConcurrentDictionary<CacheGroup, ConcurrentDictionary<CacheKey, CacheEntry>>();

            public static ConcurrentDictionary<CacheKey, CacheEntry> GetStore(CacheGroup cacheGroup)
            {
                return _stores.GetOrAdd(cacheGroup, CreateStore);
            }

            private static ConcurrentDictionary<CacheKey, CacheEntry> CreateStore(CacheGroup key)
            {
                return new ConcurrentDictionary<CacheKey, CacheEntry>(new CacheKeyComparer());
            }

            public static void ClearStore(CacheGroup cacheGroup)
            {
                ConcurrentDictionary<CacheKey, CacheEntry> store;

                if (_stores.TryGetValue(cacheGroup, out store))
                {
                    store.Clear();
                }
            }

            public static void ClearAllStores()
            {
                foreach (var cacheGroup in _stores.Keys)
                {
                    ClearStore(cacheGroup);
                }

                _stores.Clear();
            }

            public static bool ContainsGroup(CacheGroup cacheGroup)
            {
                return _stores.ContainsKey(cacheGroup);
            }

            public static IEnumerable<CacheGroup> GetAllGroups()
            {
                return _stores.Keys;
            }

            public static int GetCount(CacheGroup cacheGroup)
            {
                ConcurrentDictionary<CacheKey, CacheEntry> store;
                return _stores.TryGetValue(cacheGroup, out store) ? store.Count : 0;
            }

            public static long GetMemorySize(CacheGroup cacheGroup)
            {
                ConcurrentDictionary<CacheKey, CacheEntry> store;

                if (!_stores.TryGetValue(cacheGroup, out store))
                {
                    return 0;
                }

                return -1;
            }
        }

        private static class CacheSettingsManager
        {
            private static readonly ConcurrentDictionary<CacheGroup, CacheGroupSettings> _store = new ConcurrentDictionary<CacheGroup, CacheGroupSettings>();

            public static bool TryGet(CacheGroup cacheGroup, out CacheGroupSettings value)
            {
                return _store.TryGetValue(cacheGroup, out value);
            }

            public static void Insert(CacheGroup cacheGroup, CacheGroupSettings value)
            {
                _store.AddOrUpdate(cacheGroup, value, (key, old) => value);
            }

            public static bool Clear(CacheGroup cacheGroup)
            {
                CacheGroupSettings settings;
                return _store.TryRemove(cacheGroup, out settings);
            }

            public static void ClearAll()
            {
                _store.Clear();
            }
        }

        private static class CacheStatistics
        {
            public static long MissCount;
            public static long HitCount;

            public static void Reset()
            {
                Interlocked.Exchange(ref MissCount, 0);
                Interlocked.Exchange(ref HitCount, 0);
            }
        }

        private class CacheGroupSettings
        {
            public Bounds ConstraintBounds { get; }

            public CacheGroupSettings(Bounds constraintBounds)
            {
                ConstraintBounds = constraintBounds;
            }
        }

        private struct CacheKey
        {
            public readonly int X;
            public readonly int Y;
            public readonly int Z;

            public CacheKey(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        private class CacheKeyComparer : IEqualityComparer<CacheKey>
        {
            public bool Equals(CacheKey first, CacheKey second)
            {
                return first.X == second.X && first.Y == second.Y && first.Z == second.Z;
            }

            public int GetHashCode(CacheKey obj)
            {
                unchecked
                {
                    var hashCode = obj.X;
                    hashCode = (hashCode * 397) ^ obj.Y;
                    hashCode = (hashCode * 397) ^ obj.Z;
                    return hashCode;
                }
            }
        }

        private class CacheEntry
        {
            public GeoObjectBase Value { get; }

            public CacheEntry(GeoObjectBase value = null)
            {
                Value = value;
            }
        }
    }
}