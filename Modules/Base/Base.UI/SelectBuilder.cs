using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Base.Attributes;
using Base.ComplexKeyObjects.Unions;
using Base.UI.ViewModal;

namespace Base.UI
{


    public class StringWriter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public StringWriter Clear()
        {
            _stringBuilder.Clear();
            return this;
        }



        public StringWriter Append(string str)
        {
            _stringBuilder.Append(str);
            return this;
        }

        private string _new_line = "\n";
        private int _level = 0;

        [Conditional("DEBUG")]
        public void Push()
        {
            _level++;
            _new_line = null;
        }

        [Conditional("DEBUG")]
        public void Pop()
        {
            _level--;
            _new_line = null;
        }


        public void SetNewLine()
        {
            _new_line = _new_line ?? (_new_line = "\n" + string.Concat(Enumerable.Range(0, _level).Select(x => "\t")));
        }

        public StringWriter WriteNewLine(string str)
        {
            WriteNewLine();

            _stringBuilder.Append(str);

            return this;
        }
        public StringWriter WriteNewLine()
        {
            SetNewLine();

            _stringBuilder.Append(_new_line);

            return this;
        }



        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }



    public interface ISelectStringBuilder
    {
        void BeginObject(View view);
        void EndObject();
        void SimpleProperty(bool is_collection, string scope, string name, PropertyViewModel property_config);
        void CustomProperty(bool is_collection, string scope, string name, string property, PropertyViewModel property_config);
        void BeginComplexProperty(bool is_collection, bool check_null, string scope, string name, PropertyViewModel property_config);
        void EndComplexProperty(bool is_collection, bool check_null, string scope, string name);

        void Clear();


    }

    public class SelectStringBuilder : ISelectStringBuilder
    {

        protected readonly StringWriter Writer = new StringWriter();

        public const string DefaultScope = "it";


        public virtual void Clear()
        {
            Writer.Clear();
        }

        public void BeginObject(View view)
        {
            CheckBeginObject(true, false);

            _is_first = true;

            Writer.Append("new (");
            Writer.Push();
        }

        public void EndObject()
        {
            CheckBeginObject(false, false);

            _is_first = false;

            Writer.Pop();
            Writer.WriteNewLine(")");
        }

        private bool _is_first;

        protected void WritePropertyDelimiter()
        {
            if (_is_first)
            {
                _is_first = false;
            }
            else
            {
                Writer.Append(",");
            }

            Writer.WriteNewLine();

        }

        protected void ResetPropertyDelimiter(bool value)
        {
            _is_first = value;
        }



        public virtual void SimpleProperty(bool is_collection, string scope, string name, PropertyViewModel property_config)
        {
            CheckName(scope, name);
            WritePropertyDelimiter();
            if (property_config?.PropertyType == typeof(bool))
            {
                AppendIsNull(scope, name);
            }
            else
            {
                AppendName(scope, name);
            }
            
        }

        public virtual void CustomProperty(bool is_collection, string scope, string name, string property, PropertyViewModel property_config)
        {
            CheckName(scope, name);
            WritePropertyDelimiter();

            Writer.Append(property);
            Writer.Append(" as ");
            Writer.Append(name);

        }



        public virtual void BeginComplexProperty(bool is_collection, bool check_null, string scope, string name, PropertyViewModel property_config)
        {
            CheckBeginObject(true, true);

            CheckName(scope, name);

            WritePropertyDelimiter();

            if (check_null)
            {
                WriteCheckNull(scope, name);
            }

            if (is_collection)
            {
                AppendName(scope, name);
                Writer.Append(".Select(new (");
            }
            else
            {
                Writer.Append("new (");
            }

            Writer.Push();

            ResetPropertyDelimiter(true);
        }



        public virtual void EndComplexProperty(bool is_collection, bool check_null, string scope, string name)
        {
            CheckName(scope, name);

            CheckBeginObject(false, true);

            ResetPropertyDelimiter(false);

            Writer.Pop();

            Writer.WriteNewLine(is_collection ? ")) as " : ") as ");

            Writer.Append(name);


        }

        public override string ToString()
        {
            return Writer.ToString();

        }

        protected const string CheckNull = " == null ? null : ";

        protected void WriteCheckNull(string scope, string name)
        {
            AppendName(scope, name);
            Writer.Append(CheckNull);
        }

        protected void AppendName(string scope, string name)
        {
            Writer.Append(scope);
            Writer.Append(name);
        }
        
        protected void AppendIsNull(string scope, string name)
        {
            Writer.Append($"{scope}{name}==null ? false : {scope}{name} as {name}");
        }


        private int _check_begin = 0;

        [Conditional("DEBUG")]
        private void CheckBeginObject(bool begin, bool property)
        {

            if ((property || !begin) && _check_begin == 0)
                throw new InvalidOperationException();

            _check_begin += begin ? 1 : -1;



        }

        [Conditional("DEBUG")]
        private void CheckName(string scope, string name)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (name == null)
                throw new ArgumentNullException(nameof(name));
        }
    }


    public class SelectBuilder
    {

        private readonly ISelectStringBuilder _builder;


        private readonly View _rootView;
        private string[] _rootProps;
        private bool _check_null_collection;
        private bool _write_all_properties;
        private bool _write_discriminator = true;
        private bool _write_system = true;



        public SelectBuilder(View rootView, ISelectStringBuilder builder)
        {
            _rootView = rootView;
            _builder = builder;
        }

        public SelectBuilder WriteDiscriminator(bool write_discriminator)
        {
            _write_discriminator = write_discriminator;

            return this;
        }

        public SelectBuilder WriteSystemProperty(bool write_system)
        {
            _write_system = write_system;
            return this;
        }

        public SelectBuilder WriteAllProperties(bool write_all_properties)
        {
            _write_all_properties = write_all_properties;
            return this;
        }

        public SelectBuilder Where(string[] props)
        {
            _rootProps = props;
            return this;
        }


        public SelectBuilder CheckNullCollection(bool check = true)
        {
            _check_null_collection = check;
            return this;
        }

        public List<object> _parameters = new List<object>();

        public object[] Parameters => _parameters.ToArray();

        public string Build()
        {
            var build = false;

            _parameters.Clear();
            _builder.Clear();

            _builder.BeginObject(_rootView);

            try
            {
                WriteObject(_rootView, true, DefaultScope, _rootProps);
                build = true;
            }
            finally
            {
                if (!build)
                {
                    _parameters.Clear();
                    _builder.Clear();
                }

            }
            _builder.EndObject();

            return _builder.ToString();

        }

        private const string DefaultScope = "it.";





        string WriteDiscriminator(View view, string scope)
        {
            if (_write_discriminator)
            {
                if (view.TypeEntity.IsAssignableToGenericType(typeof(IUnionEntry<>)))
                {

                    _builder.CustomProperty(false, scope, "ID", $"{scope}.ID.ToString()+\":\"+{scope}.ExtraID", null);
                    return "ID";
                }

                //throw new NotSupportedException();

            }
            return null;

        }




        void WriteObject(View view, bool root, string scope, params string[] properties)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));



            var discriminator = WriteDiscriminator(view, scope);

            var system_properties = _write_system ? view.Config.TypeEntity.GetProperties()
                .Where(x => x.IsDefined(typeof(SystemPropertyAttribute), false)) : Enumerable.Empty<PropertyInfo>();



            if (!root || !_write_all_properties)
                system_properties = system_properties.Where(x => x.Name != "RowVersion");

            var props = view.Props;

            if (discriminator != null)
            {
                system_properties = system_properties.Where(x => x.Name != discriminator);

                props = props.Where(x => x.PropertyName != discriminator);
            }


            system_properties = system_properties.ToArray();


            if (properties.Length > 0)
                props = props.Where(x => properties.Contains(x.PropertyName) || system_properties.Any(s => s.Name == x.PropertyName));

            {
                //TODO пока так
                var sys = system_properties.Where(x => props.All(p => p.PropertyName != x.Name)).ToArray();

                foreach (var p in sys)
                {
                    WriteSystemProperty(p, scope);
                }
            }

            foreach (var property in props)
            {

                WriteProperty(property, scope);

            }

        }


        private void WriteSystemProperty(PropertyInfo property, string scope)
        {
            //TODO
            if (property.PropertyType.IsBaseObject() || property.PropertyType.IsBaseCollection())
                throw new NotSupportedException();

            WritePrimitiveProperty(scope, property.Name, null);
        }

        void WriteProperty(PropertyViewModel property, string scope)
        {

            if (property.IsPrimitive)
            {
                WritePrimitiveProperty(scope, property.PropertyName, property);
            }
            else if (property.ViewModelType.IsBaseObject() && property.PropertyType.IsBaseCollection())
            {

                if (typeof(IEasyCollectionEntry).IsAssignableFrom(property.PropertyType.GetGenericArguments()[0]))
                {
                    WriteEasyCollectionProperty(property, scope);
                }
                else
                {
                    if (property.Relationship == Relationship.OneToMany)
                        if (_write_all_properties)
                            WriteCollectionProperty(property, true, scope);
                        else
                            WriteLookupCollectionProperty(property, scope);

                    else if (property.Relationship == Relationship.ManyToMany)
                        WriteLookupCollectionProperty(property, scope);


                    else
                        throw new InvalidOperationException();
                }
            }
            else
            {
                if (property.Relationship == Relationship.One)
                    WriteLookupObjectProperty(property, scope);
                else
                {
                    if (_write_all_properties)
                        WriteObjectProperty(property, true, scope);
                    else
                        WriteLookupObjectProperty(property, scope);
                }
            }

        }

        private void WritePrimitiveProperty(string scope, string name, PropertyViewModel property_config)
        {
            _builder.SimpleProperty(false, scope, name, property_config);

        }



        void WriteLookupObjectProperty(PropertyViewModel property, string scope)
        {
            var lookup = property.ViewModelConfig.LookupProperty.Text;
            WriteObjectProperty(property, false, scope, lookup);
        }

        void WriteObjectProperty(PropertyViewModel property, bool root, string scope, params string[] properties)
        {

            var name = property.PropertyName;

            _builder.BeginComplexProperty(false, true, scope, name, property);

            WriteObject(property.ViewModelConfig.DetailView, root, GetNewScope(scope, name), properties);

            _builder.EndComplexProperty(false, true, scope, name);
        }

        void WriteLookupCollectionProperty(PropertyViewModel property, string scope)
        {
            var lookup = property.ViewModelConfig.LookupProperty.Text;
            WriteCollectionProperty(property, false, scope, lookup);
        }

        void WriteCollectionProperty(PropertyViewModel property, bool root, string scope, params string[] properties)
        {
            var name = property.PropertyName;

            _builder.BeginComplexProperty(true, _check_null_collection, scope, name, property);

            WriteObject(property.ViewModelConfig.DetailView, root, DefaultScope, properties);

            _builder.EndComplexProperty(true, _check_null_collection, scope, name);

        }

        private string GetNewScope(string scope, string name)
        {
            return scope + name + ".";
        }

        void WriteEasyCollectionProperty(PropertyViewModel property, string scope)
        {
            var name = property.PropertyName;
            var lookup = property.ViewModelConfig.LookupProperty.Text;


            _builder.BeginComplexProperty(true, _check_null_collection, scope, name, property);


            WritePrimitiveProperty(DefaultScope, "ID", null);

            _builder.BeginComplexProperty(false, true, scope, "Object", property);

            WriteObject(property.ViewModelConfig.DetailView, false, GetNewScope(DefaultScope, "Object"), lookup);

            _builder.EndComplexProperty(false, true, scope, "Object");

            _builder.EndComplexProperty(true, _check_null_collection, scope, name);


        }

    }
}