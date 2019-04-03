using System;

namespace Base.Links.Entities
{

    public interface ILink
    {
        void Init(object source, object dest);
        Type DestType { get; }
        Type SourceType { get; }
    }


    public sealed class Link<TSource, TDest> : ILink
        where TSource : class
        where TDest : class

    {
        public Action<TSource, TDest> Prepare { get; set; }

        public void Init(object source, object dest)
        {
            Prepare((TSource)source, (TDest)dest);
        }

        public Type DestType => typeof(TDest);
        public Type SourceType => typeof(TSource);
    }
}
