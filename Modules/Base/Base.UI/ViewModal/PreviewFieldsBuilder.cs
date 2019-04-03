using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Base.UI.ViewModal
{
    public class PreviewFieldsBuilder<TRoot, TProject>
        where TRoot : class
        where TProject : class
    {
        private readonly Preview _preview;
        private readonly IInitializerContext _context;

        public PreviewFieldsBuilder(Preview preview, IInitializerContext context)
        {
            _preview = preview;
            _context = context;
        }

        public PreviewFieldsBuilder<TRoot, TProject> Fields(Action<PreviewFieldsFactory<TProject>> configurator)
        {
            configurator(new PreviewFieldsFactory<TProject>(_preview.Fields));
            return this;
        }

        public PreviewFieldsBuilder<TRoot, TProject> AddExtended<TItem>(Expression<Func<TRoot, IEnumerable<TItem>>> selector,
            Action<PreviewExtraCollectionFactory<TRoot, TItem>> configurator = null)
        {

            var data = new ExtendedData(selector, typeof(TItem), _context);

            _preview.Extended.Add(data);

            configurator?.Invoke(new PreviewExtraCollectionFactory<TRoot, TItem>(_preview, data, _context));
            return this;
        }
    }


    public class PreviewExtraCollectionFactory<T, TItem>
    {
        private readonly Preview _preview;

        private readonly ExtendedData _extended_data;
        public PreviewExtraCollectionFactory(Preview preview, ExtendedData extended_data, IInitializerContext context)
        {
            _preview = preview;
            _extended_data = extended_data;


        }

        public PreviewExtraCollectionFactory<T, TItem> Order(Expression<Func<TItem, object>> order)
        {
            _extended_data.Order = order;
            return this;
        }

        public PreviewExtraCollectionFactory<T, TItem> Name(string name)
        {
            _extended_data.Name = name;
            return this;
        }
    }

}