using System;

namespace Base.Map.Clustering
{
    public class CacheSettings
    {
        public CacheSettings(string group_key, string group_title, Type type)
        {
            GroupKey = group_key;
            GroupTitle = group_title;
            Type = type;
        }

        public string GroupKey { get; }

        public string GroupTitle { get; }

        public Type Type { get;  }
    }
}