using System;

namespace Base.Map.Clustering
{
    public class CacheGroup
    {
        public CacheGroup(string key, Type type, string title)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
            Type = type;
            Title = title;
        }

        public string Key { get; }

        public string Title { get; }

        public Type Type { get; }

        public override bool Equals(object obj)
        {
            var group = obj as CacheGroup;
            return group != null && Equals(group);
        }

        protected bool Equals(CacheGroup other)
        {
            return string.Equals(Key, other.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}