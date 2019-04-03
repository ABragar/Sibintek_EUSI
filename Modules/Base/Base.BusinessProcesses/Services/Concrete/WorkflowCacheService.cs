using System;
using Base.BusinessProcesses.Services.Abstract;
using System.Collections.Generic;
using Base.Utils.Common.Caching;

namespace Base.BusinessProcesses.Services.Concrete
{

    //TODO не особо понятно как работает кэш
    public class WorkflowCacheService : IWorkflowCacheService
    {
        private readonly ISimpleCacheWrapper _cacheWrapper;

        private static readonly CacheAccessor<Dictionary<string, CacheSubCollection>> CacheKey 
            = new CacheAccessor<Dictionary<string, CacheSubCollection>>(TimeSpan.FromDays(1));

        public WorkflowCacheService(ISimpleCacheWrapper cacheWrapper)
        {
            _cacheWrapper = cacheWrapper;
            InitCache();
        }

        private Dictionary<string, CacheSubCollection> Cache
        {
            get
            {
                return _cacheWrapper.GetOrAdd(CacheKey, null, InitCache);
            }
        }

        private Dictionary<string, CacheSubCollection> InitCache()
        {
            return new Dictionary<string, CacheSubCollection>();
        }

        public string Get(string key)
        {
            if (Cache.ContainsKey(key))
            {
                var coll = Cache[key];
                return coll.Get(key);
            }
            return null;
        }

        public string Get(string key, string wfID)
        {
            if (Cache.ContainsKey(key))
            {
                var coll = Cache[key];
                return coll.Get(wfID);
            }
            return null;
        }

        public void Add(string key, string json)
        {
            var sub = getSubCollection(key);
            sub.Add(key, json);
            Cache[key] = sub;
        }

        public void Add(string key, string json, int wfID)
        {
            var sub = getSubCollection(key);
            sub.Add(wfID.ToString(), json);
            Cache[key] = sub;
        }

        public void Clear(string key)
        {
            if (Cache.ContainsKey(key))
                Cache.Remove(key);
        }

        private CacheSubCollection getSubCollection(string key)
        {
            return Cache.ContainsKey(key) ? Cache[key] : new CacheSubCollection();
        }
    }

    internal class CacheSubCollection
    {
        private static readonly object Locker = new object();
        public Dictionary<string, string> Cache = new Dictionary<string, string>();

        public string Get(string key)
        {
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            return null;
        }

        public void Add(string key, string value)
        {
            lock (Locker)
            {
                Cache[key] = value;
            }
        }

        public void Clear(string key)
        {
            lock (Locker)
            {
                Cache.Remove(key);
            }
        }

    }

    public enum WorkflowCacheType
    {
        Toolbar,
        TimeLine,
    }
}
