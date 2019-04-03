using Base.Entities.Complex;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using Base.Entities.Complex;
using Base.Extensions;
using Base.Service;

namespace Base.UI.ViewModal
{
    [DebuggerDisplay("Mnemonic={" + nameof(Mnemonic) + "}")]
    public class ViewModelConfig
    {
        protected Dictionary<Type, object> Additionals = new Dictionary<Type, object>();
        private IServiceFactory<object> _service_factory;
        private Type _service_type;
        private Type _type_entity;
        private string _type_name;

        public ViewModelConfig(Type type)
        {
            Icon = new Icon();
            ListView = new ListView();
            DetailView = new DetailView();
            Preview = new Preview();
            TypeEntity = type;
        }

        public Icon Icon { get; set; }
        public string Mnemonic { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Entity => _type_name;
        public string TypeName => _type_name;
        public string Preset { get; set; }

        public Preview Preview { get; set; }
        public DetailView DetailView { get; set; }
        public ListView ListView { get; set; }
        public LookupProperty LookupProperty { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsNotify { get; set; }

        public string LookupPropertyForUI => LookupProperty.Text;
        public string LookupPropertyForFilter => LookupProperty.Text;

        public Type ServiceType => _service_type;

        public Type TypeEntity
        {
            get { return _type_entity; }
            private set
            {
                _type_entity = value;
                _type_name = _type_entity.GetTypeName();
            }
        }

        public bool Clonable => typeof(ITransform).IsAssignableFrom(TypeEntity);

        internal void SetService(Type service_type, IServiceFactory<object> service_factory)
        {
            if (service_type == null)
                throw new ArgumentNullException(nameof(service_type));

            if (service_factory == null)
                throw new ArgumentNullException(nameof(service_factory));


            _service_factory = service_factory;
            _service_type = service_type;
        }

        internal void SetService(ViewModelConfig viewModelConfig)
        {
            if (viewModelConfig.TypeEntity != TypeEntity)
                throw new InvalidOperationException();

            _service_factory = viewModelConfig._service_factory;
            _service_type = viewModelConfig._service_type;
        }

        public T GetService<T>()
        {
            return (T)_service_factory.GetService();
        }

        public T GetAdditional<T>() where T : class
        {
            object obj;
            if (Additionals.TryGetValue(typeof(T), out obj))
            {
                return obj as T;
            }
            return default(T);
        }

        public IReadOnlyCollection<string> Relations { get; set; } = Empty;

        private static readonly string[] Empty = new string[0];

        public void SetAdditional<T>(T addin) where T : class
        {
            Additionals[typeof(T)] = addin;
        }

        public PropertyViewModel GetProperty(string property)
        {
            return ListView.Props.FirstOrDefault(x => x.PropertyName == property) ??
                   DetailView.Props.FirstOrDefault(x => x.PropertyName == property);
        }

        public ViewModelConfig Copy<T>() where T : class
        {
            return Copy(typeof(T));
        }

        public ViewModelConfig Copy(Type type = null)
        {
            if (type != null && !this.TypeEntity.IsAssignableFrom(type))
                throw new InvalidOperationException();

            var config = new ViewModelConfig(type ?? this.TypeEntity)
            {
                Icon = new Icon()
                {
                    Color = this.Icon.Color,
                    Value = this.Icon.Value
                },
                Mnemonic = this.Mnemonic,
                Name = this.Name,
                Title = this.Title,
                Preset = this.Preset,
                IsReadOnly = this.IsReadOnly,
                LookupProperty = new LookupProperty()
                {
                    Icon = this.LookupProperty.Icon,
                    Image = this.LookupProperty.Image,
                    Text = this.LookupProperty.Text
                },
                Preview = this.Preview.Copy<Preview>(),
                DetailView = this.DetailView.Copy<DetailView>(),
                ListView = this.ListView.Copy<ListView>(),
                Additionals = new Dictionary<Type, object>(this.Additionals)
            };

            if (config.TypeEntity == this.TypeEntity)
            {
                config.SetService(this);
                config.Relations = new List<string>(this.Relations);
            }

            return config;
        }

        public void Accept(IViewModelConfigVisitor visitor)
        {
            visitor.Visit(new ConfigTitle(this));
            visitor.Visit(new ConfigListViewFilter(this));
            visitor.Visit(new ConfigListView(this));

            DetailView.Editors.ForEach(editor => visitor.Visit(new ConfigEditor(editor)));
            ListView.Columns.ForEach(column => visitor.Visit(new ConfigColumn(column)));
        }

        //sib
        public void ChangeEntityType(Type type)
        { 
            if (_type_entity != null && type.IsSubclassOf(_type_entity))
            {
                _type_entity = type;
                _type_name = _type_entity.GetTypeName();
            }            
        }
        //end sib

    }

    public class ConfigTitle
    {
        private readonly ViewModelConfig _viewModelConfig;
        private string _title;

        public ConfigTitle(ViewModelConfig viewModelConfig)
        {
            _viewModelConfig = viewModelConfig;
        }

        public Icon Icon
        {
            get { return _viewModelConfig.Icon; }
            set { _viewModelConfig.Icon = value; }
        }

        public string Title
        {
            get { return _viewModelConfig.Title; }
            set { _viewModelConfig.Title = value; }
        }

        public string ListViewTitle
        {
            get { return _viewModelConfig.ListView.Title; }
            set { _viewModelConfig.ListView.Title = value; }
        }

        public string DetailViewTitle
        {
            get { return _viewModelConfig.DetailView.Title; }
            set { _viewModelConfig.DetailView.Title = value; }
        }
    }

    public class ConfigListView
    {
        private readonly ViewModelConfig _viewModelConfig;

        public ConfigListView(ViewModelConfig viewModelConfig)
        {
            _viewModelConfig = viewModelConfig;
        }

        public bool MultiEdit
        {
            get { return _viewModelConfig.ListView.MultiEdit; }
            set { _viewModelConfig.ListView.MultiEdit = value; }
        }
    }

    public class ConfigListViewFilter
    {
        private readonly ViewModelConfig _viewModelConfig;
        private string _filter;

        public ConfigListViewFilter(ViewModelConfig viewModelConfig)
        {
            _viewModelConfig = viewModelConfig;
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                try
                {
                    _viewModelConfig.ListView.DataSource.Filter =
                        DynamicExpression.ParseLambda(_viewModelConfig.TypeEntity, typeof(bool), value);

                    _filter = value;
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
    }

    public class ConfigEditor
    {
        private readonly EditorViewModel _editorViewModel;

        public ConfigEditor(EditorViewModel editorViewModel)
        {
            _editorViewModel = editorViewModel;
        }

        public string PropertyName => _editorViewModel.PropertyName;

        public string Title
        {
            get { return _editorViewModel.Title; }
            set { _editorViewModel.Title = value; }
        }

        public string Description
        {
            get { return _editorViewModel.Description; }
            set { _editorViewModel.Description = value; }
        }

        public bool Visible
        {
            get { return _editorViewModel.Visible; }
            set { _editorViewModel.Visible = value; }
        }

        public bool IsReadOnly
        {
            get { return _editorViewModel.IsReadOnly; }
            set { _editorViewModel.IsReadOnly = value; }
        }

        public bool IsRequired
        {
            get { return _editorViewModel.IsRequired; }
            set { _editorViewModel.IsRequired = value; }
        }

        public bool IsLabelVisible
        {
            get { return _editorViewModel.IsLabelVisible; }
            set { _editorViewModel.IsLabelVisible = value; }
        }

        public string TabName
        {
            get { return _editorViewModel.TabName; }
            set { _editorViewModel.TabName = value; }
        }

        public double SortOrder
        {
            get { return _editorViewModel.SortOrder; }
            set { _editorViewModel.SortOrder = value; }
        }
    }

    public class ConfigColumn
    {
        private readonly ColumnViewModel _column;

        public ConfigColumn(ColumnViewModel column)
        {
            _column = column;
        }

        public string PropertyName => _column.PropertyName;

        public string Title
        {
            get { return _column.Title; }
            set { _column.Title = value; }
        }

        public bool Visible
        {
            get { return _column.Visible; }
            set { _column.Visible = value; }
        }

        public double SortOrder
        {
            get { return _column.SortOrder; }
            set { _column.SortOrder = value; }
        }

        public bool OneLine
        {
            get { return _column.OneLine; }
            set { _column.OneLine = value; }
        }
    }

    public interface IViewModelConfigVisitor
    {
        void Visit(ConfigTitle configTitle);
        void Visit(ConfigListView configListView);
        void Visit(ConfigListViewFilter listConfigListViewFilter);
        void Visit(ConfigEditor configEditor);
        void Visit(ConfigColumn configColumn);
    }
}