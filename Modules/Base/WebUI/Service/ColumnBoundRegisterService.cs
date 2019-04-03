using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Base;
using Base.Attributes;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using Base.UI.ViewModal;
using Kendo.Mvc.UI.Fluent;
using WebUI.BoundsRegister;
using WebUI.Models;

namespace WebUI.Service
{
    public class ColumnBoundRegisterService : IColumnBoundRegisterService
    {
        private readonly Dictionary<string, ColumnBoundConfig> _bounds = new Dictionary<string, ColumnBoundConfig>();

        public BoundBuilder Register(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var key = type.FullName;

            if (_bounds.ContainsKey(key))
                throw new ArgumentException($"Bound с ключом {key} уже зарегестрирован");

            var config = new ColumnBoundConfig();
            _bounds.Add(key, config);

            return new BoundBuilder(config);
        }

        public BoundBuilder Register(PropertyDataType propertyDataType)
        {
            var key = propertyDataType.ToString();

            if (_bounds.ContainsKey(key))
                throw new ArgumentException($"Bound с ключом {key} уже зарегестрирован");

            var config = new ColumnBoundConfig();
            _bounds.Add(key, config);

            return new BoundBuilder(config);
        }

        public BoundBuilder Register(string propertyDataType)
        {
            if (_bounds.ContainsKey(propertyDataType))
                throw new ArgumentException($"Bound с ключом {propertyDataType} уже зарегестрирован");

            var config = new ColumnBoundConfig();
            _bounds.Add(propertyDataType, config);

            return new BoundBuilder(config);
        }

        public BoundBuilder Register<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            var key = CreateKeyForGridTypeAndProperty(typeof(T), propertyInfo.Name);

            if (_bounds.ContainsKey(key))
                throw new ArgumentException($"Bound с ключом {key} уже зарегестрирован");

            var config = new ColumnBoundConfig();
            _bounds.Add(key, config);

            return new BoundBuilder(config);
        }

        public BoundBuilder Register<T, TProperty>(string mnemonic, Expression<Func<T, TProperty>> property)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            var key = CreateKeyForGridTypeMnemonicAndProperty(typeof(T), mnemonic, propertyInfo.Name);

            if (_bounds.ContainsKey(key))
                throw new ArgumentException($"Bound с ключом {key} уже зарегестрирован");

            var config = new ColumnBoundConfig();
            _bounds.Add(key, config);

            return new BoundBuilder(config);
        }

        public ColumnBoundConfig GetBoundByGridTypeAndProperty(Type gridType, string propertyName)
        {
            ColumnBoundConfig result;

            var key = CreateKeyForGridTypeAndProperty(gridType, propertyName);

            _bounds.TryGetValue(key, out result);

            return result;
        }

        public ColumnBoundConfig GetBoundByGridTypeMnemonicAndProperty(Type gridType, string gridMnemonic, string propertyName)
        {
            ColumnBoundConfig result;

            var key = CreateKeyForGridTypeMnemonicAndProperty(gridType, gridMnemonic, propertyName);

            _bounds.TryGetValue(key, out result);

            return result;
        }

        public ColumnBoundConfig GetBoundByType(Type type)
        {
            ColumnBoundConfig result;

            if (type.IsEnum)
            {
                _bounds.TryGetValue(typeof (Enum).FullName, out result);
            }
            else
            {
                _bounds.TryGetValue(type.FullName, out result);
            }

            return result;
        }

        public ColumnBoundConfig GetBoundByPropertyDataTypeEnum(PropertyDataType propertyDataType)
        {
            ColumnBoundConfig result;

            _bounds.TryGetValue(propertyDataType.ToString(), out result);

            return result;
        }

        public ColumnBoundConfig GetBoundByPropertyDataTypeString(string propertyDataType)
        {
            ColumnBoundConfig result;

            _bounds.TryGetValue(propertyDataType, out result);

            return result;
        }

        public bool ExistsConfig(ColumnViewModel column, ViewModelConfig gridConfig)
        {
            if (column.PropertyType.IsEnum)
                return true;

            return _bounds.ContainsKey(CreateKeyForGridTypeMnemonicAndProperty(gridConfig.TypeEntity, gridConfig.Mnemonic, column.PropertyName)) ||
                _bounds.ContainsKey(CreateKeyForGridTypeAndProperty(gridConfig.TypeEntity, column.PropertyName)) ||
                _bounds.ContainsKey(column.PropertyType.FullName) ||
                (column.PropertyDataType.HasValue && _bounds.ContainsKey(column.PropertyDataType.Value.ToString())) ||
                _bounds.ContainsKey(column.PropertyDataTypeName);
        }

        private string CreateKeyForGridTypeAndProperty(Type gridType, string propertyName)
        {
            return gridType.FullName + "#" + propertyName;
        }

        private string CreateKeyForGridTypeMnemonicAndProperty(Type gridType, string gridMnemonic, string propertyName)
        {
            return gridType.FullName + "#" + gridMnemonic + "#" + propertyName;
        }
    }
}