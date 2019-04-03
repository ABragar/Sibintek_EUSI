using System;
using System.Linq;
using System.Reflection;
using Base.Entities.Complex;
using Base.Service;
using Base.UI.ViewModal;
using Base.Utils.Common;

namespace Base.UI
{
    public sealed class ViewModelConfigBuilder<T> : IConfigurator<T> where T : class
    {
        private readonly ViewModelConfig _config;
        private readonly IInitializerContext _context;

        public ViewModelConfig Config => _config;

        IInitializerContext IConfigurator<T>.Context => _context;

        public void SetAdditional<TAdditional>(TAdditional additional) where TAdditional : class
        {
            _config.SetAdditional(additional);
        }

        public ViewModelConfigBuilder(ViewModelConfig config, IInitializerContext context)
        {
            _config = config;
            _context = context;
        }

        public ViewModelConfigBuilder<T> Icon(Icon icon)
        {
            _config.Icon = icon;
            return this;
        }

        public ViewModelConfigBuilder<T> Icon(string icon, string color = null)
        {
            _config.Icon = new Icon
            {
                Color = color,
                Value = icon
            };
            return this;
        }

        public ViewModelConfigBuilder<T> Title(string title)
        {
            _config.Title = title;
            return this;
        }

        public ViewModelConfigBuilder<T> Name(string name)
        {
            _config.Name = name;
            return this;
        }

        public ViewModelConfigBuilder<T> IsReadOnly(bool readOnly = true)
        {
            _config.IsReadOnly = readOnly;
            return this;
        }

        public ViewModelConfigBuilder<T> LookupProperty(Action<LookupPropertyBuilder<T>> configurator)
        {
            configurator(new LookupPropertyBuilder<T>(_config.LookupProperty));
            return this;
        }

        public ViewModelConfigBuilder<T> IsNotify(bool notify = true)
        {
            _config.IsNotify = notify;
            return this;
        }



        public ViewModelConfigBuilder<T> Service<TService>() where TService : IService
        {
            var locator = _context.GetChildContext<IServiceLocator>();

            _config.SetService(typeof(TService), (IServiceFactory<object>)locator.GetService(typeof(IServiceFactory<>).MakeGenericType(typeof(TService))));
            return this;
        }

        public ViewModelConfigBuilder<T> ListView(Action<ListViewBuilder<T>> configurator)
        {
            configurator(new ListViewBuilder<T>(_config.ListView, _context));
            return this;
        }

        public ViewModelConfigBuilder<T> DetailView(Action<DetailViewBuilder<T>> configurator)
        {
            configurator(new DetailViewBuilder<T>(_config.DetailView));
            return this;
        }

        public ViewModelConfigBuilder<T> Preview(Action<PreviewBuilder<T>> configurator)
        {
            configurator( new PreviewBuilder<T>(_config.Preview, _context));
            return this;
        }

        public ViewModelConfigBuilder<T> Preview(bool enableDefaultPreview = true)
        {
            _config.Preview.Enable = enableDefaultPreview;
            return this;
        }

        public ViewModelConfigBuilder<T> WizzardDetailView(Action<WizzardDetailViewBuilder<T>> configurator)
        {
            var wizzardDetailView = ObjectHelper.CreateAndCopyObject<WizardDetailView>(_config.DetailView);

            _config.DetailView = wizzardDetailView;

            _config.DetailView.Type = DetailViewType.WizzardView;

            configurator(new WizzardDetailViewBuilder<T>(wizzardDetailView));

            return this;
        }

        public ViewModelConfigBuilder<T> Preset<TPreset>() where TPreset : class
        {
            _config.Preset = typeof(TPreset).Name;
            return this;
        }

        public ViewModelConfigBuilder<T> OnListViewAllColumns(bool all = false)
        {
            if (all)
            {
                var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                foreach (var prop in props)
                {                    
                    if ( prop.Name != "RowVersion"
                        && ! _config.ListView.Columns.Where(w => w.PropertyName == prop.Name).Any())
                        _config.ListView.Columns.Add(UI.Service.ViewModelConfigFactory.CreateColumn<T>(prop));
                }
                foreach (var col in _config.ListView.Columns)
                {
                    col.Visible = true;
                }
            }
            return this;
        }

       
    }
}
