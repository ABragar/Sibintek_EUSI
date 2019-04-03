namespace Base.Utils.Common.Caching
{
    public class CacheDependencyKey
    {
        internal CacheDependencyKey(string value)
        {
            Value = value;
        }

        public readonly string Value;
    }
}