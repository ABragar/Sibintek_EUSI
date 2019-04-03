using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Base.DAL;
using Base.Service;

namespace Base.UI.ViewModal
{
    public class PreviewBuilder<T> where T : class
    {
        private readonly Preview _preview;
        private readonly IInitializerContext _context;

        internal PreviewBuilder(Preview preview, IInitializerContext context)
        {
            _preview = preview;
            _context = context;
            _preview.Enable = true;
        }

        public PreviewBuilder<T> Enable(bool enable = true)
        {
            _preview.Enable = enable;
            return this;
        }

        public PreviewFieldsBuilder<T, T> Fields(Action<PreviewFieldsFactory<T>> configurator)
        {

            return Select((Expression<Func<T, T>>)null);
        }

        public PreviewBuilder<T> AddExtended<TItem>(Expression<Func<T, IEnumerable<TItem>>> selector, Action<PreviewExtraCollectionFactory<T, TItem>> configurator = null)
        {
            var data = new ExtendedData(selector, typeof(TItem),_context);

            _preview.Extended.Add(data);

            configurator?.Invoke(new PreviewExtraCollectionFactory<T, TItem>(_preview, data, _context));
            return this;
        }

        public PreviewFieldsBuilder<T, TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
            where TResult : class
        {
            _preview.Fields.Clear();
            _preview.CustomSelect = null;
            _preview.Selector = selector;
            _preview.SetType(typeof(TResult));

            return new PreviewFieldsBuilder<T, TResult>(_preview, _context);
        }

        public PreviewFieldsBuilder<T, TResult> Select<TResult>(Func<IUnitOfWork, T, TResult> func)
          where TResult : class
        {
            _preview.Fields.Clear();
            _preview.Selector = null;
            _preview.CustomSelect = (x, y) => func(x, (T)y);
            _preview.SetType(typeof(TResult));

            return new PreviewFieldsBuilder<T, TResult>(_preview, _context);
        }

        public PreviewFieldsBuilder<T, TResult> Select<TResult>(Func<IUnitOfWork, T,IServiceLocator ,TResult> func)
            where TResult : class
        {
            _preview.Fields.Clear();
            _preview.Selector = null;
            _preview.CustomSelect = (x, y) => func(x, (T)y, _context.GetChildContext<IServiceLocator>());
            _preview.SetType(typeof(TResult));

            return new PreviewFieldsBuilder<T, TResult>(_preview, _context);
        }


    }



}
