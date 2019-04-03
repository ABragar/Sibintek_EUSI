using System;
using System.Collections.Generic;
using System.Linq;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Excel
{
    public class LookupSelector : SelectStringBuilder
    {
        private readonly List<Property> _properties;

        public IReadOnlyCollection<Property> Properties => _properties;

        private readonly Dictionary<string, Property> _by_name;

        public LookupSelector(ViewModelConfig config, IEnumerable<string> properties)
        {
            _by_name = new Dictionary<string, Property>();
            _properties = properties.Select(x =>
            {
                var pr = config.ListView.Columns.SingleOrDefault(c => c.PropertyName == x);

                var p = new Property(x)
                {
                    Width = pr?.Width,
                    Caption = pr?.Title,
                };
                _by_name.Add(x, p);
                return p;
            }).ToList();
        }

        public class Property
        {
            public Property(string name)
            {
                Name = name;
            }

            public float? Width { get; set; }

            public string Caption { get; set; }

            public string Name { get; }

            public Type Type { get; set; }

            public string DataType { get; set; }

            public bool Ignore { get; set; }
        }

        public override void BeginComplexProperty(bool is_collection,
            bool check_null,
            string scope,
            string name,
            PropertyViewModel property_config)
        {


            CheckCollection(is_collection);


            if (check_null)
                _current_complex_property_string += scope + name + CheckNull;

            level++;
        }

        private void CheckCollection(bool is_collection)
        {
            if (is_collection)
            {
                _ignore = true;
            }

        }

        private bool _ignore;

        private int level = 0;

        private int lookup;

        private string _current_complex_property_string;

        private PropertyViewModel _current_config;

        public override void EndComplexProperty(bool is_collection, bool check_null, string scope, string name)
        {
            level--;
            if (level == 0)
            {
                AppendProperty(scope, name, _current_complex_property_string, _current_config);
                _current_config = null;
                _current_complex_property_string = null;
            }
            else
            {

            }
        }

        public void AppendProperty(string scope, string name, string property, PropertyViewModel property_config)
        {

            if (_ignore || property == null || lookup != 1)
            {
                _ignore = false;
                _by_name[name].Ignore = true;
            }
            else
            {

                var prop = _by_name[name];

                if (property_config?.ViewModelConfig?.LookupProperty?.Text != null)
                {

                    //TODO 
                    _ignore = false;
                    _by_name[name].Ignore = true;
                }

                prop.DataType = property_config?.PropertyDataTypeName;

                if (property == name)
                {
                    base.SimpleProperty(false, scope, name, property_config);
                }
                else
                {
                    base.CustomProperty(false, scope, name, property, property_config);
                }
            }
            lookup = 0;
        }


        public override void CustomProperty(bool is_collection, string scope, string name, string property, PropertyViewModel property_config)
        {
            CheckCollection(is_collection);
            lookup++;
            if (level == 0)
            {
                AppendProperty(scope, name, property, property_config);
            }
            else
            {
                _current_complex_property_string += scope + name;
                _current_config = property_config;


            }



        }

        public override void SimpleProperty(bool is_collection, string scope, string name, PropertyViewModel property_config)
        {

            CheckCollection(is_collection);
            lookup++;
            if (level == 0)
            {
                AppendProperty(scope, name, name, property_config);
            }
            else
            {
                _current_complex_property_string += scope + name;
                _current_config = property_config;
            }



        }
    }
}
