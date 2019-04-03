using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Links.Entities
{
    public interface ILinksConfigurationManager
    {
        List<ILink> Links { get; set; }

        bool CanCreateOnTheGrounOf(Type type);

    }

    public class LinksConfigurationManager : ILinksConfigurationManager
    {
        public List<ILink> Links { get; set; } //TODO: Добавить наследование

        public LinksConfigurationManager()
        {
            Links = new List<ILink>();
        }

        public bool CanCreateOnTheGrounOf(Type type)
        {
            return Links.Any(x => x.SourceType == type || x.DestType == type);
        }        
    }

    public sealed class LinkBuilder : ILinkBuilder
    {
        private readonly ILinksConfigurationManager _manager;

        public LinkBuilder(ILinksConfigurationManager manager)
        {
            _manager = manager;
        }

        public LinkConfigurationBuilder<TSource, TDest> Register<TSource, TDest>() 
            where TDest : class 
            where TSource : class
        {
            var link = new Link<TSource, TDest>();
            _manager.Links.Add(link);
            return new LinkConfigurationBuilder<TSource, TDest>(link);
        }
    }

    public sealed class LinkConfigurationBuilder<TSource, TDest>
        where TDest : class
        where TSource : class
    {
        private readonly Link<TSource, TDest> _link;
        public LinkConfigurationBuilder(Link<TSource, TDest> link)
        {
            _link = link;
        }

        public LinkConfigurationBuilder<TSource, TDest> Config(Action<TSource, TDest> action)
        {
            _link.Prepare = action;
            return this;
        }
    }
}
