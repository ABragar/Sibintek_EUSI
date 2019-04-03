using System;
using System.Collections.Generic;
using System.Linq;
using Base.Extensions;
using Base.Service;

namespace Base.ComplexKeyObjects.Common
{
    public class TypeRelationService : ITypeRelationService
    {
        private readonly ITypeNameResolver _name_resolver;


        private readonly IDictionary<Type, Item> _items = new Dictionary<Type, Item>();

        public TypeRelationService(ITypeNameResolver name_resolver)
        {
            _name_resolver = name_resolver;
        }

        public string FindName(Type type)
        {
            Item item;

            return _items.TryGetValue(type, out item) ? item.Name : null;
        }

        public IReadOnlyCollection<string> GetRelations(Type type)
        {
            Item item;

            return _items.TryGetValue(type, out item) ? item.ResolvedNames : _empty;
        }

        private static string[] _empty = new string[0];

        public void AddRelation(Type item_type, Type child_type)
        {
            var item = _items.GetOrAdd(item_type, x => new Item());

            if (item_type != child_type)
            {
                var child_item = _items.GetOrAdd(child_type, x => new Item());

                item.Relations.Add(child_item);
            }
            else
            {
                item.Reccurent = true;
            }
        }

        public void AddRelation(Type item_type, string name)
        {

            var item = _items.GetOrAdd(item_type, x => new Item());

            item.Relations.Add(new Item { Name = name, Reccurent = true });

        }

        public void ResolveNames()
        {
            foreach (var item in _items)
            {
                item.Value.Name = _name_resolver.GetName(item.Key);
            }

            foreach (var item in _items.Values)
            {
                item.ResolvedNames = item.GetAllRelations().Where(x => x.Reccurent).Select(x => x.Name).ToArray();
            }
        }

        private class Item
        {
            
            public string Name { get; set; }

            public bool Reccurent { get; set; }
            public IReadOnlyCollection<string> ResolvedNames { get; internal set; } = _empty;

            public IList<Item> Relations { get; } = new List<Item>();

            private IEnumerable<Item> Current()
            {
                yield return this;
            }

            public IEnumerable<Item> GetAllRelations()
            {
                return Current().Concat(Relations.SelectMany(x => x.GetAllRelations()));
            }
        }
    }
}