using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Base.Attributes;
using Base.Extensions;
using Base.UI.ViewModal;

namespace Base.UI
{
    [DebuggerDisplay("Title={" + nameof(Title) + "}")]
    public abstract class PropertyViewModel : BaseObject 
    {
        private Type _type;
        private static int _increment;

        protected PropertyViewModel()
        {
            UID = $"pr_{unchecked((uint)Interlocked.Increment(ref _increment))}";
            CanRead = true;
        }

        protected PropertyViewModel(string name) : this()
        {
            PropertyName = name;
        }
        
        public string SysName { get; set; }
        public bool IsPrimitive { get; set; }
        public string UID { get; }
        public string PropertyName { get; set; }
        public string Mnemonic { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; } = true;
        public int? MaxLength { get; set; }
        public bool IsSystemPropery { get; set; }
        public Type PropertyType { get; set; }
        public PropertyDataType? PropertyDataType { get; set; }
        public string PropertyDataTypeName { get; set; }
        public Dictionary<string, string> Params { get; set; }
        public Relationship Relationship { get; set; }
        public bool CanRead { get; set; }

        public Type ViewModelType
        {
            get
            {
                if (_type != null) return _type;

                if (PropertyType == null) return null;

                if (PropertyType.IsBaseCollection())
                {
                    _type = PropertyType.GetGenericArguments()[0];

                    if (typeof(IEasyCollectionEntry).IsAssignableFrom(_type))
                        _type = PropertyType.GetEntryOfUnboundedTypeOfCollection(typeof(EasyCollectionEntry<>));
                }
                else
                {
                    _type = PropertyType;
                }

                return _type;
            }
        }
         
        public ViewModelConfig ViewModelConfig { get; set; }
        public ViewModelConfig ParentViewModelConfig { get; set; }

        public virtual T Copy<T>(T propertyView = null) where T: PropertyViewModel, new()
        {
            var pr = propertyView ?? new T();
            pr.SysName = SysName;
            pr.IsPrimitive = IsPrimitive;
            pr.PropertyName = PropertyName;
            pr.Mnemonic = Mnemonic;
            pr.Title = Title;
            pr.Description = Description;
            pr.Visible = Visible;
            pr.MaxLength = MaxLength;
            pr.IsSystemPropery = IsSystemPropery;
            pr.PropertyType = PropertyType;
            pr.PropertyDataType = PropertyDataType;
            pr.PropertyDataTypeName = PropertyDataTypeName;
            pr.Relationship = Relationship;
            pr.Params = Params?.ToDictionary(x => x.Key, x => x.Value);
            pr.ViewModelConfig = ViewModelConfig;
            pr.ParentViewModelConfig = ParentViewModelConfig;           
            return pr;
        }
    }
}
