using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WordTemplates.Core;

namespace WebUI.Binders
{
    public class InheritanceModelBinderProvider : IModelBinderProvider
    {
        private readonly ModelBinderDictionary _dictionary;

        public InheritanceModelBinderProvider(ModelBinderDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        private readonly ConcurrentDictionary<Type, IModelBinder> _binders = new ConcurrentDictionary<Type, IModelBinder>();

        private readonly List<Tuple<Func<Type, bool>, IModelBinder>> _settings = new List<Tuple<Func<Type, bool>, IModelBinder>>();

        public void Add(Func<Type, bool> predicat, IModelBinder binder)
        {
            if (_binders.Any())
                throw new InvalidOperationException();

            _settings.Add(Tuple.Create(predicat, binder));
        }

        public IModelBinder GetBinder(Type modelType)
        {

            return _dictionary.GetOrDefault(modelType) ?? _binders.GetOrAdd(modelType, x =>
            {
                return _settings.FirstOrDefault(s=>s.Item1(x))?.Item2; 
                
            });
        }
    }
}