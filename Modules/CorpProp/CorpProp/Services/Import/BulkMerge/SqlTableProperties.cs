using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorpProp.Services.Import.BulkMerge
{
    public class SqlTableProperties
    {
        #region Protected Fields

        protected List<PropertyInfo> _properties;

        #endregion Protected Fields

        #region Private Fields

        private Type _type;

        #endregion Private Fields

        #region Public Constructors

        public SqlTableProperties(Type type)
        {
            _properties = new List<PropertyInfo>();
            _type = type;
        }

        #endregion Public Constructors

        #region Public Properties

        public Type BaseType => _type;

        public IEnumerable<PropertyInfo> Properties
        {
            get
            {
                return _properties;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public virtual void Add(PropertyInfo propertyInfo)
        {
            _properties.Add(propertyInfo);
        }

        #endregion Public Methods
    }
}
