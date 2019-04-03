using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Base.Attributes;
using Base.ComplexKeyObjects;
using Base.ComplexKeyObjects.Unions;
using Base.Entities.Complex;
using Base.Enums;
using Base.Exceptions;
using Base.Extensions;
using Base.Service;
using Base.UI.Helpers;
using Base.UI.ViewModal;
using Base.Utils.Common.Maybe;

namespace Base.UI.Service
{
    public static class ViewModelConfigFactory
    {
        public static ViewModelConfig CreateDefault(Type type, IServiceLocator locator)
        {
            if (type.IsPrimitive || type.IsValueType || type.IsEnum || type == typeof(string) || type == typeof(Guid))
                return null;

            var viewModelConfigAttribute = type.GetCustomAttribute<ViewModelConfigAttribute>() ??
                                           new ViewModelConfigAttribute()
                                           {
                                               Title = type.Name
                                           };

            var vmc = new ViewModelConfig(type)
            {
                Mnemonic = type.GetTypeName(),
                DetailView = new DetailView(),
                LookupProperty = new LookupProperty()
                {
                    Text = viewModelConfigAttribute.LookupProperty ??
                           (type.GetProperty("Title") ?? type.GetProperty("Name") ?? type.GetProperty("ID"))?.Name
                },
                Title = viewModelConfigAttribute.Title,
                Icon = new Icon() { Value = viewModelConfigAttribute.Icon },
                IsReadOnly = viewModelConfigAttribute.IsReadOnly
            };


            InitDefaultServiceType(vmc, locator);

            InitDetailView(vmc);
            InitListView(vmc);
            InitPreview(vmc);

            return vmc;
        }


        private static void InitDefaultServiceType(ViewModelConfig config, IServiceLocator locator)
        {
            var type = config.TypeEntity;


            var service_type = type.Assembly.GetTypes()
                .Where(x => typeof(IService).IsAssignableFrom(x) && x.IsInterface)
                .FirstOrDefault(x => x.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericArguments()[0] == type));


            if (service_type != null && TrySetService(config, locator, service_type))
                return;


            if (type.IsAssignableToGenericType(typeof(IUnionEntry<>)) &&
                TrySetService(config, locator, typeof(IUnionService<>).MakeGenericType(type)))
                return;


            if (typeof(HCategory).IsAssignableFrom(type) &&
                TrySetService(config, locator, typeof(IBaseCategoryService<>).MakeGenericType(type)))
                return;


            if (typeof(ICategorizedItem).IsAssignableFrom(type) &&
                TrySetService(config, locator, typeof(IBaseCategorizedItemService<>).MakeGenericType(type)))
                return;


            if (typeof(IBaseObject).IsAssignableFrom(type) &&
                TrySetService(config, locator, typeof(IBaseObjectService<>).MakeGenericType(type)))
                return;
        }

        public static bool TrySetService(ViewModelConfig config, IServiceLocator locator, Type service_type)
        {
            try
            {
                config.SetService(service_type, (IServiceFactory<object>)
                    locator.GetService(typeof(IServiceFactory<>).MakeGenericType(service_type)));

                return true;
            }
            catch (ActivationException)
            {
                return false;
            }
        }

        public static void InitDetailView(ViewModelConfig viewModelConfig)
        {
            var type = viewModelConfig.TypeEntity;

            viewModelConfig.DetailView.Editors =
                type.GetProperties().Where(x => x.IsDefined(typeof(DetailViewAttribute), false))
                    .Select(x => CreateEditor(type, x))
                    .Where(x => x != null).ToList();
        }

        public static void InitListView(ViewModelConfig viewModelConfig)
        {
            var type = viewModelConfig.TypeEntity;

            if (typeof(ICategorizedItem).IsAssignableFrom(type))
            {
                var categoryType =
                    viewModelConfig.TypeEntity.GetProperties()
                        .FirstOrDefault(pr => typeof(ITreeNode).IsAssignableFrom(pr.PropertyType))
                        .PropertyType;

                viewModelConfig.ListView = new ListViewCategorizedItem()
                {
                    MnemonicCategory = categoryType.GetTypeName()
                };
            }
            else if (typeof(HCategory).IsAssignableFrom(type))
                viewModelConfig.ListView = new TreeView();
            else if (typeof(IScheduler).IsAssignableFrom(type))
                viewModelConfig.ListView = new SchedulerView();
            else if (typeof(IGantt).IsAssignableFrom(type))
                viewModelConfig.ListView = new GanttView();
            else
                viewModelConfig.ListView = new ListView();


            viewModelConfig.ListView.Columns =
                type.GetProperties().Where(x => x.IsDefined(typeof(ListViewAttribute), false))
                    .Select(x => CreateColumn(type, x))
                    .Where(x => x != null).ToList();
        }

        public static void InitPreview(ViewModelConfig viewModelConfig)
        {
            viewModelConfig.DetailView.Editors.Where(
                    x =>
                        viewModelConfig.ListView.Columns.Where(col => col.Visible)
                            .Any(col => col.PropertyName == x.PropertyName))
                .ForEach(edt => { viewModelConfig.Preview.Fields.Add(edt.ToObject<PreviewField>()); });
        }

        public static EditorViewModel CreateEditor(Type type, PropertyInfo prop)
        {
            var detailViewAttr = prop.GetCustomAttribute<DetailViewAttribute>() ?? new DetailViewAttribute();
            bool hideLabel = detailViewAttr.HideLabel;

            if (!hideLabel)
                hideLabel = string.IsNullOrEmpty(detailViewAttr.Name) && !string.IsNullOrEmpty(detailViewAttr.TabName);

            EditorViewModel editor;

            if (Attribute.IsDefined(prop, typeof(ImageAttribute)))
            {
                var imageAttribute = prop.GetCustomAttribute<ImageAttribute>();

                editor = new ImageEditorViewModel(prop.Name)
                {
                    Width = imageAttribute.Width,
                    Height = imageAttribute.Height,
                    Crop = imageAttribute.Crop,
                    SelectFileStorage = imageAttribute.SelectFileStorage,
                    Upload = imageAttribute.Upload,
                    Circle = imageAttribute.Circle,
                    DefaultImage = imageAttribute.DefaultImage,
                    PropertyDataTypeName = "Image"
                };
            }
            else
            {
                editor = new EditorViewModel(prop.Name);
            }

            //TODO: 
            
            editor.Title = detailViewAttr?.Name ?? prop.Name;
            editor.IsLabelVisible = !hideLabel;
            editor.TabName = detailViewAttr.TabName; 
            editor.IsReadOnly = !prop.CanWrite || detailViewAttr.ReadOnly;

            editor.IsRequired = detailViewAttr.Required ||
                                (!editor.IsReadOnly && prop.PropertyType != typeof(bool) &&
                                 prop.PropertyType.IsValueType && !prop.PropertyType.IsNullable());

            editor.SortOrder = detailViewAttr.Order;
            editor.Visible = detailViewAttr.Visible;
            editor.BgColor = detailViewAttr.BgColor;

            editor.Group = detailViewAttr.Group; 

            InitPropertyVm(type, editor, prop);

            return editor;
        }

        public static EditorViewModel CreateEditor<T>(PropertyInfo prop) where T : class
        {
            return CreateEditor(typeof(T), prop);
        }

        public static ColumnViewModel CreateColumn(Type type, PropertyInfo prop)
        {
            var listViewAttr = prop.GetCustomAttribute<ListViewAttribute>() ?? new ListViewAttribute();
            var detailViewAttr = prop.GetCustomAttribute<DetailViewAttribute>();

            ColumnViewModel column = null;

            var imageAttribute = prop.GetCustomAttribute<ImageAttribute>();

            if (imageAttribute != null)
            {
                column = new ImageColumnViewModel()
                {
                    ImageWidth = imageAttribute.WidthForListView,
                    ImageHeight = imageAttribute.HeightForListView,
                    Circle = imageAttribute.Circle,
                    DefaultImage = imageAttribute.DefaultImage,
                    PropertyDataTypeName = "Image"
                };
            }
            else
            {
                column = new ColumnViewModel();
            }

            column.Title = listViewAttr.Name ?? detailViewAttr?.Name ?? prop.Name;
            column.Visible = !listViewAttr.Hidden;
            column.SortOrder = listViewAttr.Order == Int32.MinValue ? (detailViewAttr?.Order ?? 0) : listViewAttr.Order;
            column.Filterable = listViewAttr.Filterable;
            column.Width = listViewAttr.Width != 0 ? (int?)listViewAttr.Width : null;
            column.Height = listViewAttr.Height != 0 ? (int?)listViewAttr.Height : null;
            column.Groupable = listViewAttr.Groupable;
            column.Sortable = listViewAttr.Sortable;
            column.OneLine = listViewAttr.OneLine;

            InitPropertyVm(type, column, prop);

            return column;
        }

        public static ColumnViewModel CreateColumn<T>(PropertyInfo prop)
        {
            return CreateColumn(typeof(T), prop);
        }

        public static void InitPropertyVm(Type type, PropertyViewModel pr, PropertyInfo prop)
        {
            if (prop != null)
            {
                pr.PropertyName = prop.Name;
                pr.PropertyType = prop.PropertyType;
                pr.IsSystemPropery = prop.IsDefined(typeof(SystemPropertyAttribute), false);

                var detailViewAttr = prop.GetCustomAttribute<DetailViewAttribute>();
                var maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();

                var propertyDataType = prop.GetCustomAttribute<PropertyDataTypeAttribute>();
                var imageAttribute = prop.GetCustomAttribute<ImageAttribute>();

                pr.MaxLength = maxLengthAttr?.Length;
                pr.Description = detailViewAttr?.Description;
                pr.PropertyDataType = propertyDataType?.DataType;
                pr.Params = ViewModelConfigHelper.GetEditorParams(propertyDataType?.Params);

                if (pr.PropertyDataType == null)
                {
                    if (pr.PropertyType == typeof(string) && pr.PropertyName == nameof(IComplexKeyObject.ExtraID) &&
                        typeof(IComplexKeyObject).IsAssignableFrom(type))
                        pr.PropertyDataType = PropertyDataType.ExtraId;
                }

                if (pr.PropertyDataType != null)
                    pr.PropertyDataTypeName = pr.PropertyDataType != PropertyDataType.Custom
                        ? pr.PropertyDataType.ToString()
                        : propertyDataType?.CustomDataType;
            }

            if (pr.PropertyType == null)
                throw new Exception("Property type is null");

            pr.Mnemonic = pr.Mnemonic ?? (pr.PropertyType.IsBaseObject() ? pr.PropertyType.GetTypeName() : null);
            pr.Relationship = GetRelationship(type, pr);
            pr.IsPrimitive = pr.Relationship == Relationship.None;

            if (pr.PropertyDataTypeName == null)
            {
                var vmType = pr.ViewModelType;

                if (vmType == typeof(string) && pr.MaxLength == null)
                    pr.PropertyDataTypeName = "MultilineText";
                else if (vmType == typeof(int) || vmType == typeof(int?) || vmType == typeof(long) || vmType == typeof(long?))
                    pr.PropertyDataTypeName = "Integer";
                else if (vmType == typeof(decimal) || vmType == typeof(decimal?))
                    pr.PropertyDataTypeName = "Decimal";
                else if (vmType == typeof(double) || vmType == typeof(double?))
                    pr.PropertyDataTypeName = "Double";
                else if (vmType == typeof(bool) || vmType == typeof(bool?))
                    pr.PropertyDataTypeName = "Boolean";
                else if (vmType == typeof(DateTime) || vmType == typeof(DateTime?))
                    pr.PropertyDataTypeName = "Date";
                else if (vmType.IsEnum())
                    pr.PropertyDataTypeName = "Enum";
                else if (vmType == typeof(Period))
                    pr.PropertyDataTypeName = "Period";
                else if (vmType == typeof(Icon))
                    pr.PropertyDataTypeName = "Icon";
                else if (vmType == typeof(Url))
                    pr.PropertyDataTypeName = "ComplexUrl";
                else if (vmType == typeof(LinkBaseObject))
                    pr.PropertyDataTypeName = "LinkBaseObject";
                else if (vmType == typeof(MultiEnum))
                    pr.PropertyDataTypeName = "MultiEnum";
                else if (vmType == typeof(Phone))
                    pr.PropertyDataTypeName = "Phone";
                else if (vmType == typeof(RemindPeriod))
                    pr.PropertyDataTypeName = "RemindPeriod";
                else
                {
                    switch (pr.Relationship)
                    {
                        case Relationship.OneToMany:
                            if (typeof(FileCollectionItem).IsAssignableFrom(pr.PropertyType.GetGenericArguments()[0]))
                                pr.PropertyDataTypeName = "Files";
                            else if (typeof(IEasyCollectionEntry).IsAssignableFrom(pr.PropertyType.GetGenericArguments()[0]))
                                pr.PropertyDataTypeName = "EasyCollection";
                            else
                                pr.PropertyDataTypeName = "OneToMany";
                            break;

                        case Relationship.ManyToMany:
                            pr.PropertyDataTypeName = "ManyToMany";
                            break;

                        case Relationship.One:
                            pr.PropertyDataTypeName = "BaseObjectOne";
                            break;

                        case Relationship.None:
                            pr.PropertyDataTypeName = vmType.Name;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private static Relationship GetRelationship(Type type, PropertyViewModel pr)
        {
            if (pr.PropertyType == null)
                return Relationship.None;

            if (pr.PropertyType.IsBaseObject())
            {
                return Relationship.One;
            }
            else if (pr.PropertyType.IsGenericType)
            {
                var collectionType = pr.PropertyType.GetGenericType();

                var genericType = collectionType?.GenericTypeArguments[0];

                if (genericType != null && genericType.IsBaseObject())
                {
                    IEnumerable<PropertyInfo> props =
                        genericType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    if (props.Any(x =>
                    {
                        var gType = x.PropertyType.GetGenericType();
                        return gType != null && gType.GenericTypeArguments[0].IsAssignableFrom(type);
                    }))
                    {
                        return Relationship.ManyToMany;
                    }

                    return Relationship.OneToMany;
                }
            }

            return Relationship.None;
        }
    }
}